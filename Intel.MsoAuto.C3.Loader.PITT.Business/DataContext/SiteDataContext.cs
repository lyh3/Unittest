using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.DataAccess;
using System.Reflection;
using log4net;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using MongoDB.Driver;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{
    public class SiteDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public SiteDataContext() { }

        public void SyncApplicableSiteMappingsData()
        {
            log.Info("--> SyncApplicableSiteMappingsData" + "(" + Settings.Env + ")");
            ISqlDataAccess dataAccess = null;
            try
            {
                log.Info("Connecting...");

                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.C3ConnectionString, Settings.SiteMappingSP);
                dataAccess.ExecuteReader();
                log.Info("Connected...");
                SiteMappings data = new SiteMappings();
                SiteMapping d = null;
                while (dataAccess.DataReader.Read())
                {
                    d = new SiteMapping();
                    d.Id = dataAccess.DataReader["Id"].ToStringSafely();
                    d.Site = dataAccess.DataReader["Site"].ToStringSafely();
                    d.Process = dataAccess.DataReader["Process"].ToStringSafely();
                    d.SystemName = dataAccess.DataReader["SystemName"].ToStringSafely();
                    data.Add(d);
                }
                log.Info("Total number of records from SiteMappings: " + data.Count);
                SaveApplicableSiteMappingsToPitt(data);
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
            log.Info("<-- SyncApplicableSiteMappingsData" + "(" + Settings.Env + ")");

        }

        private void SaveApplicableSiteMappingsToPitt(SiteMappings siteMappings)
        {
            IMongoCollection<PittSiteMapping> SiteMappingsCollection = new MongoDataAccess().GetMongoCollection<PittSiteMapping>(Constants.PITT_SITE_MAPPING);
            IMongoCollection<ExternalSiteProcessMapping> ExternalSitecollection = new MongoDataAccess().GetMongoCollection<ExternalSiteProcessMapping>(Constants.APPLICABLE_SITES_MAPPING);
            IClientSessionHandle clientSession = new MongoDataAccess().GetClientSession();
            
            clientSession.StartTransaction();
            try
            {
            List<ExternalSiteProcessMapping> externalSiteProcessMapping = new List<ExternalSiteProcessMapping>();
            foreach (var siteMapping in siteMappings)
            {
      
                List<PittSiteMapping>? refMappings = SiteMappingsCollection.Find(clientSession, x => x.siteMappings.Any(t => (siteMapping.SystemName == t.externalSystemName) && (t.siteName == siteMapping.Site))).ToList();

                foreach (var refMapping in refMappings)
                {
                    if (refMapping != null)
                    {
                        string? process = null;

                        string? processStr = null;

                        //Make process readable for each system
                        switch (siteMapping.SystemName)
                        {
                            case "WSPW":
                                process = siteMapping.Process;
                                break;
                        }


                        if (process.IsNotNull())
                        {
                           
                            //Check if Site mapping process and site name is already a value if not add new parent Site mapping
                            if (!externalSiteProcessMapping.Any(s => s.siteName == refMapping?.siteName && s.process == process))
                            {
                                List<PittSiteMappingItem> newSiteItems = new List<PittSiteMappingItem>();

                                    foreach (PittSiteMappingItem site in refMapping.siteMappings)
                                {
                      
                                    newSiteItems.Add(new PittSiteMappingItem
                                    {
                                        externalSystemName = site.externalSystemName,
                                        siteName = site.siteName
                                    });

                                }

                                ExternalSiteProcessMapping pittSiteMapping = new ExternalSiteProcessMapping
                                {
                                    siteName = refMapping?.siteName,
                                    createdOn = DateTime.UtcNow,
                                    updatedOn = DateTime.UtcNow,
                                    process = process,
                                    siteMappings = newSiteItems,
                                };
                                externalSiteProcessMapping.Add(pittSiteMapping);
                            }
                            //else
                            //{
                            //    //Check if Site mapping process and site name is already a value if so add into site mapping
                            //    int externalSiteProcessMappingIndex = externalSiteProcessMapping.FindIndex(c => c.siteName == refMapping?.siteName && c.process == process);                            
                            //    // if the same site name and system is already within the site mappings don't add it
                            //    if (externalSiteProcessMappingIndex != -1)
                            //    {
                            //        if (!externalSiteProcessMapping[externalSiteProcessMappingIndex].siteMappings.Any(s => s.siteName == siteMapping.Site && s.externalProjectName == siteMapping.SystemName))
                            //        {
                            //            PittSiteMappingItem newItem = new PittSiteMappingItem
                            //            {
                            //                externalProjectName = siteMapping.SystemName,
                            //                siteName = siteMapping.Site
                            //            };
                            //            externalSiteProcessMapping[externalSiteProcessMappingIndex].siteMappings.Add(newItem);
                            //        }                             
                            //    }                             
                            //}
                        }
                      
                    }
                }
                //If the external mapping exist for that process and pitt eqvilent site name then append it to the current external site object otherwise create a new one

            }

                ExternalSitecollection.DeleteMany(clientSession, _ => true);
                ExternalSitecollection.InsertMany(clientSession, externalSiteProcessMapping);
                clientSession.CommitTransaction();
            }
            catch
            {
                clientSession.AbortTransaction();
                // TODO: Log error.
                throw;
            }

        }
    }
}
