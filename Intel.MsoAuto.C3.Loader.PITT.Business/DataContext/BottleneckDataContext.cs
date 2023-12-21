using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Reflection;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.PITT.Business.Models.Bottlenecks.YieldAnalysis;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;
using Intel.MsoAuto.C3.PITT.Business.Models.Bottlenecks.Interfaces;
using log4net;
using MongoDB.Driver;
using Intel.MsoAuto.C3.PITT.Business.Models.Global;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{
    public class BottleneckDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly int workWeekLookAhead = 13;

        public BottleneckDataContext() { }

        public void SyncYieldAnalysisForecastItemsAsync()
        {
            log.Info("--> SyncYieldAnalysisForecastItems" + "(" + Settings.Env + ")");

            IClientSessionHandle clientSession = new MongoDataAccess().GetClientSession();
            clientSession.StartTransaction();

            try
            {
                IMongoCollection<StateModelAssociationProject> collection = new MongoDataAccess().GetMongoCollection<StateModelAssociationProject>(Constants.PITT_STATE_MODEL_ASSOCIATION_PROJECTS, clientSession);
                IMongoCollection<YieldAnalysisBottleneck> bottleneckCollection = new MongoDataAccess().GetMongoCollection<YieldAnalysisBottleneck>(Constants.BOTTLENECKS, clientSession);
                IMongoCollection<Project> projectCollection = new MongoDataAccess().GetMongoCollection<Project>(Constants.PITT_PROJECTS, clientSession);
                IMongoCollection<YieldAnalysisForecastItem> yaCollection = new MongoDataAccess().GetMongoCollection<YieldAnalysisForecastItem>(Constants.YIELD_ANALYSIS_FORECAST_ITEMS, clientSession);


                //Get all workflows with Waiting YA
                var filter = Builders<StateModelAssociationProject>.Filter.ElemMatch(z => z.stateModelAssociation.stateModel.states, a => a.id == Constants.WAITING_YA_ID);

                List<StateModelAssociationProject> yieldAnalysisForecastItems =  collection.Find(clientSession, filter).ToList();

                //Init the forecast array that will go in mongo
                List<YieldAnalysisForecastItem> yaForecastItems = new List<YieldAnalysisForecastItem>();

                //Get today's year and week
                int yearToday = DateTime.Parse(DateTime.UtcNow.ToString()).Year;
                int weekToday = ISOWeek.GetWeekOfYear(DateTime.UtcNow);

                //Get YA bottlnecks so we can get sites
                YieldAnalysisBottleneck yieldAnalysisBottleneck = bottleneckCollection.Find(clientSession, x => x.type == BottleneckTypeEnum.YieldAnalysis).FirstOrDefault();
                foreach(Threshold threshold in yieldAnalysisBottleneck.details.thresholds) {

                    //Add this weeks forecast item
                    yaForecastItems.Add(new YieldAnalysisForecastItem
                    {
                        projects = new C3.PITT.Business.Models.Global.KeyValuePairs(),
                        siteName = threshold.siteName,
                        week = weekToday,
                        year = yearToday,
                        numOfProjects = 0,
                        workWeek="WW"+weekToday.ToString() + " " + yearToday.ToString(),
                        createdBy="system",
                        createdOn=DateTime.UtcNow,
                        updatedBy="system",
                        updatedOn=DateTime.UtcNow,
                    });

                    //Add the next 13 work week forecast items
                    for (int i = 1; i <= workWeekLookAhead; i++)
                    {
                        //Calculate next work week and account if next year
                        int workWeekCaluclated = (weekToday + i) > 52 ? (weekToday + i) - 52 : (weekToday + i);
                        int yearCalculated = (weekToday + i) > 52 ? (yearToday + 1) : yearToday;
                        yaForecastItems.Add(new YieldAnalysisForecastItem
                        {
                            projects = new KeyValuePairs(),
                            siteName = threshold.siteName,
                            week = workWeekCaluclated, 
                            year = yearCalculated,
                            numOfProjects = 0,
                            workWeek = "WW" + workWeekCaluclated.ToString() + " " + yearCalculated,
                            createdBy = "system",
                            createdOn = DateTime.UtcNow,
                            updatedBy = "system",
                            updatedOn = DateTime.UtcNow,
                        });
                    }
                }

                //Iterate through all Waiting YA projects
                foreach (StateModelAssociationProject item in yieldAnalysisForecastItems)
                {
                    //get the ya state from the association and state model
                    State? waitingYaState = item.stateModelAssociation.stateModel.states.Find(state => state.id == Constants.WAITING_YA_ID);

                    if (waitingYaState != null && waitingYaState.startDate != null)
                    {
                        //convert the state to year and week
                        int year = DateTime.Parse(waitingYaState.startDate.ToString()!).Year;
                        int week = ISOWeek.GetWeekOfYear((DateTime)waitingYaState.startDate!);

                        //NOTE: we do this check because looking for a project is expensive so we should check if it is worth it before we do the project check
                        int yaForecastItemDateIndex = yaForecastItems.FindIndex(x => x.week == week && x.year == year);
                        //If matching date then search for the site from project
                        if (yaForecastItemDateIndex != -1)
                        {
                            //Find the project
                            Project foundProject = projectCollection.Find(clientSession, x => x.id == item.projectId).FirstOrDefault();
                            if (foundProject != null)
                            {
                                //Find all the sites and date from our init YA forecast items
                                List<YieldAnalysisForecastItem> foundYaForecastItems = yaForecastItems.FindAll(x => x.week == week && x.year == year && foundProject.sitesAffected?.Contains(x.siteName) == true);
                                foreach(YieldAnalysisForecastItem foundYaForecastItem in foundYaForecastItems)
                                {
                                    int foundYaForecastItemIndex = yaForecastItems.FindIndex(x => x.week == foundYaForecastItem.week && x.year == foundYaForecastItem.year && x.siteName == foundYaForecastItem.siteName);

                                    if (yaForecastItems[foundYaForecastItemIndex].projects != null && foundYaForecastItemIndex != -1 )
                                    {
                                        yaForecastItems[foundYaForecastItemIndex].projects!.Add(new C3.PITT.Business.Models.Global.KeyValuePair {
                                            key = foundProject.id,
                                            value = foundProject.projectProcessId
                                        });
                                        yaForecastItems[foundYaForecastItemIndex].numOfProjects = yaForecastItems[foundYaForecastItemIndex].projects!.Count();
                                    }
                              
                                }
                            }
                        }
                    }
                }
            
                //Insert into collection
                 var deletedResults = yaCollection.DeleteMany(clientSession, _ => true);
                 yaCollection.InsertMany(clientSession, yaForecastItems);
                clientSession.CommitTransaction();

            }
            catch (Exception ex)
            {
                clientSession.AbortTransaction();
                log.Error(ex.ToString());
                throw new Exception(ex.ToString());
            }

            log.Info("<-- SyncYieldAnalysisForecastItems" + "(" + Settings.Env + ")");
        }

    }
}
