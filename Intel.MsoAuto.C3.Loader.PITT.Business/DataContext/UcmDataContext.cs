using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.DataAccess;
using System.Reflection;
using log4net;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using MongoDB.Driver;
using Intel.MsoAuto.C3.PITT.Business.Models;
using MongoDB.Bson;
using System.Data;
using Intel.MsoAuto.C3.PITT.Business.Models.Interfaces;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{
    public class UcmDataContext
    {
        private const string _UCM = "UCM";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UcmDataContext() { }

        public void SyncUcmProjectCriticalityData()
        {
            log.Info("--> SyncUcmProjectCriticalityData" + "(" + Settings.Env +")" );
            ISqlDataAccess dataAccess = null;
            try
            {
                log.Info("Connecting...");

                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.C3ConnectionString, Settings.ProjectCriticalitySp);
                dataAccess.ExecuteReader();
                log.Info("Connected...");
                List<Entities.ProjectCriticality> data = new List<Entities.ProjectCriticality>();
                Entities.ProjectCriticality d = null;
                while (dataAccess.DataReader.Read())
                {
                    d = new Entities.ProjectCriticality();             
                    d.shortName = dataAccess.DataReader["ShortName"].ToStringSafely();
                    d.name = dataAccess.DataReader["Name"].ToStringSafely();
                    d.isActive = dataAccess.DataReader["Active"].ToStringSafely() == "True" ? true : false;
                    data.Add(d);
                }
                log.Info("Total number of records from ProjectCriticality: " + data.Count);

                SaveOrUpdateProjectCriticalityToPitt(data);

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }
            log.Info("<-- SyncUcmProjectCriticalityData" + "(" + Settings.Env + ")");

        }

        public void SyncUcmDetailedChangeReasonsData()
        {
            log.Info("--> SyncUcmDetailedChangeReasonsData" + "(" + Settings.Env + ")");
            ISqlDataAccess dataAccess = null;
            try
            {
                log.Info("Connecting...");

                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.C3ConnectionString, Settings.DetailedChangeReaonsSp);
                dataAccess.ExecuteReader();
                log.Info("Connected...");
                List<Entities.DetailedChangeReason> data = new List<Entities.DetailedChangeReason>();
                Entities.DetailedChangeReason d = null;
                while (dataAccess.DataReader.Read())
                {
                    d = new Entities.DetailedChangeReason();
                    d.name = dataAccess.DataReader["Name"].ToStringSafely();
                    d.isActive = dataAccess.DataReader["Active"].ToStringSafely() == "True" ? true : false;
                    data.Add(d);
                }
                log.Info("Total number of records from DetailedChangeReason: " + data.Count);

                SaveOrUpdateDetailedChangeReasonsToPitt(data);

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }
            log.Info("<-- SyncUcmDetailedChangeReasonsData" + "(" + Settings.Env + ")");

        }

        public void SyncUcmData()
        {
            log.Info("--> StartUcmDailyTask" + "(" + Settings.Env + ")");
            try
            {
                List<Entities.Ucm> ucms = GetNewUpdatedRecords();
                UpdateUcmsToUcmPitt(ucms);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw new Exception(ex.ToString());
            }
            log.Info("<-- StartUcmDailyTask" + "(" + Settings.Env + ")");
        }

        private void SaveOrUpdateProjectCriticalityToPitt(List<Entities.ProjectCriticality> pcs)
        {
            IMongoCollection<Entities.ProjectCriticality> collection = new MongoDataAccess().GetMongoCollection<Entities.ProjectCriticality>(Constants.PITT_PROJECT_CRITICALITIES);
            List<Entities.ProjectCriticality> projectCriticalitiesPitt = collection.Find(_ => true).ToList();

            IClientSessionHandle clientSession = new MongoDataAccess().GetClientSession();

            clientSession.StartTransaction();
            try
            {
                foreach (Entities.ProjectCriticality pc in pcs)
                {
                    Entities.ProjectCriticality? found = projectCriticalitiesPitt.Find(p => p.name == pc.name);

                    if (found != null)
                    {
                        if (!(pc.isActive == found.isActive && found.shortName == found.shortName))
                        {
                            found.updatedOn = DateTime.UtcNow;
                            found.isActive = pc.isActive;
                            found.shortName = pc.shortName;
                            collection.DeleteOne(clientSession, x => x.name == pc.name);
                            collection.InsertOne(clientSession, found);
                        }
                    }
                    else
                    {
                        pc.updatedOn = DateTime.UtcNow;
                        pc.createdOn = DateTime.UtcNow;
                        collection.InsertOne(clientSession, pc);
                    }
                }

                // If UCM has a deleted row then soft delete
                foreach (Entities.ProjectCriticality pc in projectCriticalitiesPitt)
                {
                    Entities.ProjectCriticality? found = pcs.Find(p => p.name == pc.name);

                    if (found == null)
                    {
                        pc.updatedOn = DateTime.UtcNow;
                        pc.isActive = false;
                        collection.DeleteOne(clientSession, x => x.name == pc.name);
                        collection.InsertOne(clientSession, pc);
                    }
                }
                clientSession.CommitTransaction();
            }
            catch
            {
                clientSession.AbortTransaction();
                // TODO: Log error.
                throw;
            }
        }



        private void SaveOrUpdateDetailedChangeReasonsToPitt(List<Entities.DetailedChangeReason> dcrs)
        {
            IMongoCollection<Entities.DetailedChangeReason> collection = new MongoDataAccess().GetMongoCollection<Entities.DetailedChangeReason>(Constants.PITT_DETAILED_CHANGE_REASONS);
            List<Entities.DetailedChangeReason> pittDetailedChangeReason = collection.Find(_ => true).ToList();
            IClientSessionHandle clientSession = new MongoDataAccess().GetClientSession();

            clientSession.StartTransaction();
            try
            {
                foreach (Entities.DetailedChangeReason pc in dcrs)
                {
                    Entities.DetailedChangeReason? found = pittDetailedChangeReason.Find(p => p.name == pc.name);

                    if (found != null)
                    {
                        if (!(pc.isActive == found.isActive))
                        {
                            found.updatedOn = DateTime.UtcNow;
                            found.isActive = pc.isActive;
                            collection.DeleteOne(clientSession, x => x.name == pc.name);
                            collection.InsertOne(clientSession, found);
                        }
                    }
                    else
                    {
                        pc.updatedOn = DateTime.UtcNow;
                        pc.createdOn = DateTime.UtcNow;
                        collection.InsertOne(clientSession, pc);
                    }
                }

                // If UCM has a deleted row then soft delete
                foreach (Entities.DetailedChangeReason pc in pittDetailedChangeReason)
                {
                    Entities.DetailedChangeReason? found = dcrs.Find(p => p.name == pc.name);

                    if (found == null)
                    {
                        pc.updatedOn = DateTime.UtcNow;
                        pc.isActive = false;
                        collection.DeleteOne(clientSession, x => x.name == pc.name);
                        collection.InsertOne(clientSession, pc);
                    }
                }
                clientSession.CommitTransaction();
            }
            catch
            {
                clientSession.AbortTransaction();
                // TODO: Log error.
                throw;
            }

        }

        /*
        * Find the difference between the UCM Collection in PITT with the new changes from C3
        * IF changes found, update the UCM Collection and update the projects with the UCM Values to the C3 new values
        */
        private void UpdateUcmsToUcmPitt(List<Entities.Ucm> newUcms)
        {
            IClientSessionHandle clientSession = new MongoDataAccess().GetClientSession();

            clientSession.StartTransaction();
            try
            {
                IMongoCollection<Entities.Ucm> ucmCollection = new MongoDataAccess().GetMongoCollection<Entities.Ucm>(Constants.PITT_UCM, clientSession);
                List<Entities.Ucm> pittUcms = ucmCollection.Find(_ => true).ToList();

                //For all new UCMS we need to find the eqvialent UCM in PITT
                foreach (Entities.Ucm newU in newUcms)
                {

                    Entities.Ucm? ucm = pittUcms.Find(u => u.changeId == newU.changeId && u.processName == newU.processName);

                    if (ucm != null)
                    {
                        newU.updatedOn = DateTime.UtcNow;
                        newU.createdOn = ucm.createdOn;
                        newU.id = ucm.id;
                        ucmCollection.DeleteOne(clientSession, u => u.id == ucm.id);
                        ucmCollection.InsertOne(clientSession, newU);
                        Entities.UcmPitt ucmpitt = GetPittValuesFromUcm(newU);
                        UpdateUcmsPittToProjectsPitt(ucmpitt);
                    }
                    else if (ucm == null && pittUcms.Where(x => x.changeId == newU.changeId).Any() && newU.processId.IsNeitherNullNorEmpty())
                    {
                        CreateUcmWithNewProcess(newU, clientSession);
                    }
                }
                clientSession.CommitTransaction();
            }
            catch
            {
                clientSession.AbortTransaction();
                // TODO: Log error.
                throw;
            }
        }


        /*
         * Update all projects with the same UCM ID to the formated UCM Object
         */
        private void UpdateUcmsPittToProjectsPitt(Entities.UcmPitt ucm)
        {
            IMongoCollection<Project> projectCollection = new MongoDataAccess().GetMongoCollection<Project>(Constants.PITT_PROJECTS);
            projectCollection.UpdateMany(x => x.ucm.id == ucm.id,
                Builders<Project>.Update
                .Set(p => p.projectCriticality, ucm.changeCriticality)
                .Set(p => p.ucm, ucm)
                .Set(p => p.currentSourcingSupplier, ucm.supplierName)
                .Set(p => p.ipnSpnCurrent, ucm.ipnImpacted)
                .Set(p => p.reasonForChange, ucm.reasonForChange)
                .Set(p => p.sitesAffected, ucm.qualLocation)
                );
        }

        /*
        * Format UCM to PITT Ucm values
        */
        private Entities.UcmPitt GetPittValuesFromUcm(Entities.Ucm ucm)
        {

            IMongoCollection<Entities.Process> processCollection = new MongoDataAccess().GetMongoCollection<Entities.Process>(Constants.PITT_PROCESS);
            IMongoCollection<PittSiteMapping> siteMappingCollection = new MongoDataAccess().GetMongoCollection<PittSiteMapping>(Constants.PITT_SITE_MAPPING);
            Entities.UcmPitt group = new Entities.UcmPitt();
            Entities.Process? process = new Entities.Process();

            //Parse UCM values for formatting  

            // Retrieve the corresponding PITT process name for the UCM process name given.
            if (!string.IsNullOrEmpty(ucm.processName))
            {
                List<Entities.Process> processes = new List<Entities.Process>();
                var builder = Builders<Entities.Process>.Filter;
                FilterDefinition<Entities.Process> filter = builder.Eq(x => x.isActive, true);
                filter &= builder.Where(x => x.processMappings != null && x.processMappings.Count(y => y.systemName.ToUpper() == _UCM && y.processName.ToUpper() == ucm.processName.ToUpper()) > 0);
                processes.AddRange(processCollection.Find(filter).ToList());
                process = processes.IsNotNull() && processes.Count > 0 ? processes.FirstOrDefault() : null;
            }

            List<string>? ucmIpnImpacted = ucm.ipnImpacted.IsNeitherNullNorEmpty() ? ucm.ipnImpacted?.Split(',').ToList() : null;
            List<string>? qualLocation = null;
            if (ucm.qualLocation != null)
            {
                List<PittSiteMapping> result = siteMappingCollection.Find(site => site.siteMappings.Any((x) => (x.externalSystemName == _UCM && ucm.qualLocation.Contains(x.siteName)))).ToList();
                qualLocation = new List<string>();
                foreach (PittSiteMapping x in result)
                {
                    if (x.siteName.IsNotNull())
                    {
                        qualLocation.Add(x.siteName);
                    }
                }
            }
            List<string>? reasonForChange = ucm.reasonForChange.IsNeitherNullNorEmpty() ? ucm.reasonForChange?.Split(',').ToList() : null;
            List<string>? detailedReasonForChange = ucm.detailedReasonForChange.IsNeitherNullNorEmpty() ? ucm.detailedReasonForChange?.Split(',').ToList() : null;

            //UCM to PITT Setters
            group.id = ucm?.id;
            group.changeId = ucm.changeId;
            group.changeCriticality = ucm?.changeCriticality;
            group.reasonForChange = reasonForChange;
            group.detailedReasonForChange = detailedReasonForChange;
            group.materialsAvailableFromSupplierDate = ucm?.materialsAvailableFromSupplierDate;
            group.supplierConversionDate = ucm?.supplierConversionDate;
            group.mainSupplierContact = ucm?.mainSupplierContact;
            group.intelResponseName = ucm?.intelResponseName;
            group.supplierId = ucm?.supplierId;
            group.supplierName = ucm?.supplierName;
            group.ipnImpacted = ucmIpnImpacted?.ToList();
            group.process = process;
            group.xccbBinderNumber = ucm?.xccbBinderNumber;
            group.qualLocation = qualLocation;
            group.changeDescription = ucm?.changeDescription;
            group.createdOn = ucm?.createdOn;
            group.updatedOn = ucm?.updatedOn;


            return group;

        }

        /*
        * Get updated records that were stored in PITT
        */
        private List<Entities.Ucm> GetNewUpdatedRecords()
        {
            ISqlDataAccess dataAccess = null;
            List<Entities.Ucm> ucms = null;
            IMongoCollection<Entities.Ucm> ucmCollection = new MongoDataAccess().GetMongoCollection<Entities.Ucm>(Constants.PITT_UCM);
            List<Entities.Ucm> pittUcms = ucmCollection.Find(_ => true).ToList();
            DataTable dt = new DataTable();

            // Define the structure of the DataTable by adding columns with the appropriate names and data types.

            dt.Columns.Add("ChangeID", typeof(string));
            dt.Columns.Add("ChangeCriticality", typeof(string));
            dt.Columns.Add("ReasonforChange", typeof(string));
            dt.Columns.Add("DetailedReasonForChange", typeof(string));
            dt.Columns.Add("MaterialsAvailableFromSupplierDate", typeof(string));
            dt.Columns.Add("SupplierConversionDate", typeof(string));
            dt.Columns.Add("MainSupplierContact", typeof(string));
            dt.Columns.Add("IntelResponseName", typeof(string));
            dt.Columns.Add("SupplierId", typeof(string));
            dt.Columns.Add("SupplierName", typeof(string));
            dt.Columns.Add("LastUpdatedOn", typeof(string));
            dt.Columns.Add("IpnImpacted", typeof(string));
            dt.Columns.Add("ChangeDescription", typeof(string));
            dt.Columns.Add("ProcessId", typeof(string));
            dt.Columns.Add("ProcessName", typeof(string));
            dt.Columns.Add("XCCBBinderNumber", typeof(string));
            dt.Columns.Add("QualLocation", typeof(string));


            // Loop through the UcmRecord objects in the 'data' collection, and add each record to the DataTable as a new row.
            foreach (Entities.Ucm d in pittUcms)
            {
                dt.Rows.Add(
                    d.changeId,
                    d.changeCriticality,
                    d.reasonForChange,
                    d.detailedReasonForChange,
                    d.materialsAvailableFromSupplierDate.ToString(),
                    d.supplierConversionDate.ToString(),
                    d.mainSupplierContact,
                    d.intelResponseName,
                    d.supplierId,
                    d.supplierName,
                    null,
                    d.ipnImpacted,
                    d.changeDescription,
                    d.processId,
                    d.processName,
                    null,
                    d.qualLocation
                );
            }

            try
            {
                log.Info("Connecting...");
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.C3ConnectionString, Settings.UcmTableDiffSp);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", "[stage].[IUcmRecord]", dt);
                dataAccess.ExecuteReader();
                log.Info("Connected...");
                List<Entities.Ucm> ucmResult = null;
      
                if (dataAccess.DataReader.IsNotNull())
                {
                    ucms = new List<Entities.Ucm>();
                    while (dataAccess.DataReader.Read())
                    {
                        Entities.Ucm? ucm = null;
                        ucm = new Entities.Ucm
                        {
                            changeId = dataAccess.DataReader["ChangeID"].ToStringSafely(),
                            changeCriticality = dataAccess.DataReader["ChangeCriticality"].ToStringSafely(),
                            reasonForChange = dataAccess.DataReader["ReasonforChange"].ToStringSafely(),
                            detailedReasonForChange = dataAccess.DataReader["DetailedReasonForChange"].ToStringSafely(),
                            materialsAvailableFromSupplierDate = dataAccess.DataReader["MaterialsAvailableFromSupplierDate"].ToStringSafely() != "" ? DateTime.SpecifyKind(DateTime.Parse(dataAccess.DataReader["MaterialsAvailableFromSupplierDate"].ToStringSafely()), DateTimeKind.Utc) : null,
                            supplierConversionDate = dataAccess.DataReader["MaterialsAvailableFromSupplierDate"].ToStringSafely() != "" ? DateTime.SpecifyKind(DateTime.Parse(dataAccess.DataReader["SupplierConversionDate"].ToStringSafely()), DateTimeKind.Utc) : null,
                            mainSupplierContact = dataAccess.DataReader["MainSupplierContact"].ToStringSafely(),
                            intelResponseName = dataAccess.DataReader["IntelResponseName"].ToStringSafely(),
                            supplierId = dataAccess.DataReader["SupplierId"].ToStringSafely(),
                            supplierName = dataAccess.DataReader["SupplierName"].ToStringSafely(),
                            ipnImpacted = dataAccess.DataReader["IpnImpacted"].ToStringSafely(),
                            changeDescription = dataAccess.DataReader["ChangeDescription"].ToStringSafely(),
                            processId = dataAccess.DataReader["ProcessId"].ToStringSafely(),
                            processName = dataAccess.DataReader["ProcessName"].ToStringSafely(),
                            qualLocation = dataAccess.DataReader["QualLocation"].ToStringSafely()
                        };
                        ucms.Add(ucm);
                    }
                }
                log.Info("Total number of updated UCM records: " + ucms.Count);
     
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.InnerException);
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }
            return ucms;
        }

        private void CreateUcmWithNewProcess(Entities.Ucm ucm, IClientSessionHandle clientSession)
        {
            try
            {
                // Get the new process from PITT. If no match found return.
                IMongoCollection<Entities.Process> processCollection = new MongoDataAccess().GetMongoCollection<Entities.Process>(Constants.PITT_PROCESS);
                Entities.Process? process = processCollection.Find(x => x.processMappings != null && x.processMappings.Count(y => y.systemName == "UCM" && y.processName == ucm.processName) > 0).FirstOrDefault();
                if (process == null)
                    return;

                IMongoCollection<Entities.Ucm> ucmCollection = new MongoDataAccess().GetMongoCollection<Entities.Ucm>(Constants.PITT_UCM, clientSession);
                IMongoCollection<BsonDocument> projectCollection = new MongoDataAccess().GetMongoCollection<BsonDocument>(Constants.PITT_PROJECTS, clientSession);

                // Retrieve list of active projects with same UCM change ID.
                var builder = Builders<BsonDocument>.Filter;
                FilterDefinition<BsonDocument> filter = builder.Eq("ucm.changeId", ucm.changeId);
                filter &= builder.Eq("isActive", true);
                filter &= builder.Eq("isDraft", false);
                List<BsonDocument> results = projectCollection.Find(filter).ToList();

                if (results.Count > 0)
                {
                    ucm.createdOn = DateTime.UtcNow;
                    ucm.updatedOn = DateTime.UtcNow;

                    // Create UCM document with the new process.
                    ucmCollection.InsertOne(clientSession, ucm);

                    // Get distinct base IDs for projects that contain matching UCM change ID.
                    List<BsonDocument> baseProjects = results.AsQueryable().DistinctBy(y => y["baseId"]).ToList();

                    foreach (BsonDocument baseProject in baseProjects) 
                    {
                        var builder2 = Builders<BsonDocument>.Filter;
                        FilterDefinition<BsonDocument> filter2 = builder2.Eq("ucm.changeId", ucm.changeId);
                        filter2 &= builder2.Eq("isActive", true);
                        filter2 &= builder2.Eq("isDraft", false);
                        filter2 &= builder2.Eq("baseId", baseProject["baseId"]);
                        List<BsonDocument> results2 = projectCollection.Find(filter2).Sort(Builders<BsonDocument>.Sort.Descending("updatedOn")).ToList();

                        // Clone project document with new process.
                        BsonDocument newProject = new BsonDocument();
                        BsonDocument sourceProject = results2.First();
                        newProject = sourceProject.DeepClone().ToBsonDocument();
                        newProject["_id"] = ObjectId.GenerateNewId();
                        newProject["process"] = process.ToBsonDocument();
                        newProject["isProcessSpecific"] = true;

                        if (sourceProject["process"] != null)
                            newProject["parentProcessId"] = sourceProject["process"]["_id"].ToString();

                        newProject["projectProcessId"] = newProject["baseId"].ToString() + "-" + process.name;
                        newProject["systemGeneratedName"] = newProject["baseId"].ToString() + " : " + "System Default Project Name";
                        newProject["status"] = "draft";
                        newProject["parentProjectId"] = sourceProject["_id"].ToString();

                        // Set the UCM object and update site values.
                        Entities.UcmPitt ucmpitt = GetPittValuesFromUcm(ucm);
                        newProject["ucm"] = ucmpitt.ToBsonDocument();
                        newProject["sitesAffected"] = ucmpitt.qualLocation != null && ucmpitt.qualLocation.Count > 0 ? new BsonArray(ucmpitt.qualLocation): null;
                        newProject["pilotSites"] = ucmpitt.qualLocation != null && ucmpitt.qualLocation.Count > 0 ? new BsonArray(ucmpitt.qualLocation) : null;

                        // Reset certain fields to null for new project.
                        newProject["stateModelAssociation"] = BsonNull.Value;
                        newProject["pilotXccbDocumentNum"] = BsonNull.Value;
                        newProject["pilotXccbHorizonNum"] = BsonNull.Value;
                        newProject["pilotXccbCeids"] = BsonNull.Value;
                        newProject["finalXccbDocumentNum"] = BsonNull.Value;
                        newProject["finalXccbHorizonNum"] = BsonNull.Value;
                        newProject["finalXccbCeids"] = BsonNull.Value;

                        newProject["lifeCycleStatus"] = LifeCycleStatus.DRAFT;
                        newProject["isDraft"] = true;
                        newProject["isActive"] = true;
                        newProject["createdBy"] = "system";
                        newProject["createdOn"] = DateTime.UtcNow;
                        newProject["updatedBy"] = "system";
                        newProject["updatedOn"] = DateTime.UtcNow;

                        projectCollection.InsertOne(clientSession, newProject);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create UCM records with new process.\n\n" + ex.Message);
            }
        }
    }
}


