using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.C3.PITT.Business.Models;
using Intel.MsoAuto.C3.PITT.Business.Models.Bottlenecks.Interfaces;
using Intel.MsoAuto.C3.PITT.Business.Models.Bottlenecks.YieldAnalysis;
using Intel.MsoAuto.C3.PITT.Business.Models.Global;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;
using MongoDB.Driver;
using System.Globalization;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.C3.PITT.Business.Models.Lists;
using MongoDB.Driver.Linq;

namespace Intel.MsoAuto.C3.MailService.Notification.DataContext {
    internal class NotificationMongoDbContext {
        private readonly ReadOnlyMongoDataAccess _readOnlyMongoDbAccess;
        public NotificationMongoDbContext()
        {
            _readOnlyMongoDbAccess = new ReadOnlyMongoDataAccess(Settings.Configuration);
        }
        public NotificationArDetails GetArAssigned()
        {
            return QueryArAssignedDetails();
        }
        public async Task<NotificationArDetails> GetArAssignedAsync()
        {
            NotificationArDetails details = new NotificationArDetails();
            await Task.Run(() => {details = QueryArAssignedDetails();});
            return details;
        }
        public Dictionary<string, NotificationData<BottleneckNotificationDatum>> GetBottleneckforeCasts()
        {
            return QueryBottleneckForeCasts();
        }
        public async Task<Dictionary<string, NotificationData<BottleneckNotificationDatum>>> GetBottleneckForeCastsAsyc()
        {
            Dictionary<string, NotificationData<BottleneckNotificationDatum>> results = new Dictionary<string, NotificationData<BottleneckNotificationDatum>>();
            await Task.Run(() => { results = QueryBottleneckForeCasts();});
            return results;
        }
        public Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>> GetWorkflowStatus()
        {
            return QueryStateDetailsNotificationWorkflowStatuses();
        }
        public async Task<Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>>> GetWorkflowStatusAsync()
        {
            Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>> statuses = new Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>>();
            await Task.Run(() =>
            {
                statuses = QueryStateDetailsNotificationWorkflowStatuses();
            });
            return statuses;
        }
        private Dictionary<string, NotificationData<BottleneckNotificationDatum>> QueryBottleneckForeCasts()
        {
            Dictionary<string, NotificationData<BottleneckNotificationDatum>> results =  new Dictionary<string, NotificationData<BottleneckNotificationDatum>>();
            try
            {
                List<YieldAnalysisForecastItem> yaForecastItems = new List<YieldAnalysisForecastItem>();
                IMongoCollection<StateModelAssociationProject> collection = _readOnlyMongoDbAccess.GetMongoCollection<StateModelAssociationProject>(PITT.Business.Core.Constants.STATE_MODEL_ASSOCIATIONS_PROJECT_NAME);
                IMongoCollection<YieldAnalysisBottleneck> bottleneckCollection = _readOnlyMongoDbAccess.GetMongoCollection<YieldAnalysisBottleneck>(Core.Constants.BOTTLENECKS);
                IMongoCollection<BaseProject> projectCollection = _readOnlyMongoDbAccess.GetMongoCollection<BaseProject>(PITT.Business.Core.Constants.PROJECTS_COLLECTION_NAME);
                IMongoCollection<YieldAnalysisForecastItem> yaCollection = _readOnlyMongoDbAccess.GetMongoCollection<YieldAnalysisForecastItem>(PITT.Business.Core.Constants.YIELD_ANALYSIS_FORECAST_ITEMS);
                var filter = Builders<StateModelAssociationProject>.Filter.ElemMatch(x => x.stateModelAssociation.stateModel.states, a => a.id == Constants.WAITING_YA_ID);
                var yieldAnalysisForecastItems = collection.Find(filter).ToList();
                int yearToday = DateTime.Parse(DateTime.UtcNow.ToString()).Year;
                int weekToday = ISOWeek.GetWeekOfYear(DateTime.UtcNow);
                YieldAnalysisBottleneck yieldAnalysisBottleneck = bottleneckCollection.Find(x => x.type == BottleneckTypeEnum.YieldAnalysis).FirstOrDefault();
                foreach (Threshold threshold in yieldAnalysisBottleneck.details.thresholds)
                {
                    //Add this weeks forecast item
                    yaForecastItems.Add(new YieldAnalysisForecastItem {
                        projects = new KeyValuePairs(),
                        siteName = threshold.siteName,
                        week = weekToday,
                        year = yearToday,
                        numOfProjects = 0,
                        workWeek = $"{Constants.WORK_WEEK}{ weekToday }  { yearToday }",
                        createdBy = Constants.SYSTEM,
                        createdOn = DateTime.UtcNow,
                        updatedBy = Constants.SYSTEM,
                        updatedOn = DateTime.UtcNow,
                    });
                    //Add the next 13 work week forecast items
                    for (int i = 1; i <= Constants.WORK_WEEK_LOOK_AHEAD; i++)
                    {
                        //Calculate next work week and account if next year
                        int workWeekCaluclated = (weekToday + i) > Constants.WEEKS_OF_A_YEAR ? (weekToday + i) - Constants.WEEKS_OF_A_YEAR : (weekToday + i);
                        int yearCalculated = (weekToday + i) > Constants.WEEKS_OF_A_YEAR ? (yearToday + 1) : yearToday;
                        yaForecastItems.Add(new YieldAnalysisForecastItem {
                            projects = new KeyValuePairs(),
                            siteName = threshold.siteName,
                            week = workWeekCaluclated,
                            year = yearCalculated,
                            numOfProjects = 0,
                            workWeek = $"{Constants.WORK_WEEK}{weekToday}  {yearToday}",
                            createdBy = Constants.SYSTEM,
                            createdOn = DateTime.UtcNow,
                            updatedBy = Constants.SYSTEM,
                            updatedOn = DateTime.UtcNow,
                        });
                    }
                    foreach (StateModelAssociationProject item in yieldAnalysisForecastItems)
                    {
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
                                var foundProject = projectCollection.Find(x => x.id == item.projectId).FirstOrDefault();
                                if (foundProject != null)
                                {
                                    //Find all the sites and date from our init YA forecast items
                                    List<YieldAnalysisForecastItem> foundYaForecastItems = yaForecastItems.FindAll(x => x.week == week && x.year == year && foundProject.sitesAffected?.Contains(x.siteName) == true);
                                    foreach (var foundYaForecastItemIndex in from foundYaForecastItem in foundYaForecastItems
                                                                             let foundYaForecastItemIndex = yaForecastItems.FindIndex(x => x.week == foundYaForecastItem.week && x.year == foundYaForecastItem.year && x.siteName == foundYaForecastItem.siteName)
                                                                             where yaForecastItems[foundYaForecastItemIndex].projects != null && foundYaForecastItemIndex != -1
                                                                             select foundYaForecastItemIndex)
                                    {
                                        yaForecastItems[foundYaForecastItemIndex].projects!.Add(new PITT.Business.Models.Global.KeyValuePair {
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
                results = TransformBottleneckData(yaForecastItems, projectCollection);
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            return results;
        }
        private static Dictionary<string, NotificationData<BottleneckNotificationDatum>> 
            TransformBottleneckData(IEnumerable<YieldAnalysisForecastItem> yaForecastItems, 
                                                IMongoCollection<BaseProject> projectCollection)
        {
            Dictionary<string, NotificationData<BottleneckNotificationDatum>> results = new Dictionary<string, NotificationData<BottleneckNotificationDatum>>();
            string emailTemplateName = (new NotificationData<BottleneckNotificationDatum>().NotificationTemplateName);
            foreach (YieldAnalysisForecastItem item in yaForecastItems.Where(x => x.projects?.Count > 0).ToList())
            {
                if (null != item.projects)
                {
                    foreach (var p in item.projects)
                    {
                        var project = projectCollection.Find(x => x.id == p.key).FirstOrDefault();
                        UserSimplified projectOwner = project.owner;
                        string sendTo = projectOwner.email;                      
                        string pId = project.id.ToStringSafely();
                        if (!results.ContainsKey(sendTo))
                            results.Add(sendTo, new NotificationData<BottleneckNotificationDatum>());
                        NotificationData<BottleneckNotificationDatum> forecasts = results[sendTo];
                        BottleneckNotificationDatum datum = new BottleneckNotificationDatum {
                            ProjectId = pId,
                            ProjectProcessId = project.projectProcessId,
                            Email = sendTo,
                            UserName = projectOwner.name,
                            Wwid = projectOwner.wwid,
                            Idsid = projectOwner.idsid,
                            //--- forecast info ---
                            siteName = item.siteName,
                            week = item.week,
                            year = item.year,
                            workWeek = $"WW{item.week} {item.year}",
                            createdBy = item.createdBy,
                            updatedBy = item.updatedBy,
                            //--- project info ----
                            project = project,
                        };
                        if (forecasts.Lookup(datum) == false) 
                            forecasts.Add(datum);
                    }
                }
            }
            return results;
        }
        private NotificationArDetails QueryArAssignedDetails()
        {
            NotificationArDetails details = new NotificationArDetails();
            try
            {
                Processes processes = new Processes();
                IMongoCollection<Process> processCollection = _readOnlyMongoDbAccess.GetMongoCollection<Process>(PITT.Business.Core.Constants.PROCESS_COLLECTION_NAME);
                FilterDefinitionBuilder<Process> processessBuilder = Builders<Process>.Filter;
                var processessFilter = Builders<Process>.Filter.Eq(x => x.isActive, true);
                processes.AddRange(processCollection.Find(processessFilter).ToList());

                ActionRequests actionRequests = new ActionRequests();
                IMongoCollection<ActionRequest> actionRequestCollection = _readOnlyMongoDbAccess.GetMongoCollection<ActionRequest>(PITT.Business.Core.Constants.ACTION_REQUESTS_COLLECTION_NAME);
                var builder = Builders<ActionRequest>.Filter;
                FilterDefinition<ActionRequest> filter = builder.Eq(x => x.isActive, true);
                actionRequests.AddRange(actionRequestCollection.Find(filter).ToList());

                IMongoCollection<BaseProject> projectCollection = _readOnlyMongoDbAccess.GetMongoCollection<BaseProject>(PITT.Business.Core.Constants.PROJECTS_COLLECTION_NAME);
                var projectBuilder = Builders<BaseProject>.Filter;
                FilterDefinition<BaseProject> projectFilter = projectBuilder.Eq(x => x.isActive, true);
                BaseProjects projects = new BaseProjects();
                projects.AddRange(projectCollection.Find(projectFilter).ToList());

                var docs = from a in actionRequests.AsQueryable()
                           join j in projects on a.projectId equals j.id
                           join p in processes.AsQueryable() on a.processId equals p.id into proc
                           from p2 in proc.DefaultIfEmpty()
                           select new NotificationArDetail() {
                               id = a.id,
                               task = a.task,
                               projectId = a.projectId,
                               processId = j.projectProcessId,
                               assignedTo = a.assignedTo,
                               assignedBy = a.assignedBy,
                               status = a.status,
                               expectedCompletionDate = a.expectedCompletionDate,
                               comments = a.comments,
                               startDate = a.startDate,
                               endDate = a.endDate,
                               featureId = a.featureId,
                               isActive = a.isActive,
                               createdBy = a.createdBy,
                               createdOn = a.createdOn,
                               updatedBy = a.updatedBy,
                               updatedOn = a.updatedOn,
                               projectName = string.IsNullOrEmpty(j.name) ? j.systemGeneratedName + "" : j.name + "",
                               Idsid = j.owner.idsid,
                               AR = a.task,
                               Comments = a.comments
                           };
                details.AddRange(docs.ToList());
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            return details;
        }
        private Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>> QueryStateDetailsNotificationWorkflowStatuses()
        {
            Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>> dic = new Dictionary<string, NotificationData<ProgressionStatusNotificationDatum>>();
            try
            {
                List<ProgressionStatusNotificationDatum> wfStatuses = new List<ProgressionStatusNotificationDatum>();
                IMongoCollection<StateModelAssociationProject> stModelAssociateionsCollection = _readOnlyMongoDbAccess.GetMongoCollection<StateModelAssociationProject>(PITT.Business.Core.Constants.STATE_MODEL_ASSOCIATIONS_PROJECT_NAME);
                IMongoCollection<BaseProject> projectsCollection = _readOnlyMongoDbAccess.GetMongoCollection<BaseProject>(PITT.Business.Core.Constants.PROJECTS_COLLECTION_NAME);

                foreach (StateModelAssociationProject sa in stModelAssociateionsCollection.AsQueryable().ToList())
                {
                    foreach (State st in sa.stateModelAssociation.stateModel.states)
                    {
                        if (st.status == Constants.ACTIVE_STATUS)
                        {
                            var docs = (from pj in projectsCollection.AsQueryable()
                                    where pj.id == sa.projectId
                                    select new ProgressionStatusNotificationDatum {
                                        ProjectId = pj.id,
                                        ProjectProcessId = pj.projectProcessId,
                                        ProgressionStatus = st.name,
                                        StateId = st.id,
                                        Email = pj.owner.email,
                                        Idsid = pj.owner.idsid,
                                        UserName = pj.owner.name,
                                        project = pj
                                    });
                            wfStatuses.AddRange(docs.ToList());
                            break;
                        }
                    }
                }
                foreach (ProgressionStatusNotificationDatum item in wfStatuses)
                {
                    if (!dic.Keys.Contains(item.Email))
                        dic.Add(item.Email, new NotificationData<ProgressionStatusNotificationDatum>());
                    NotificationData<ProgressionStatusNotificationDatum> notificationData = dic[item.Email];
                    if (notificationData.Lookup(item) == false)
                        notificationData.Add(item);
                }
            }
            catch (Exception ex)
            {
                Shared.Functions.LogMyError(ex, this);
                throw;
            }
            return dic;
        }
    }
}
