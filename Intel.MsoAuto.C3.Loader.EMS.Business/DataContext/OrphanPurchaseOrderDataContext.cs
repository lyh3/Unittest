using Intel.MsoAuto.C3.Loader.EMS.Business.Entities;
using Intel.MsoAuto.C3.Loader.EMS.Business.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Reflection;
using log4net;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess;

namespace Intel.MSOAuto.C3.Loader.EMS.Business.DataContext
{
    internal class OrphanPurchaseOrderDataContext : IOrphanPurchaseOrderDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public OrphanPurchaseOrderDataContext()
        {

        }

        public void SyncNeedsData()
        {
            log.Info("--> SyncNeedsData");

            ISqlDataAccess dataAccess = null;
            IDataReader dataReader = null;
            Dictionary<String, String> needidPoidDict = new Dictionary<String, String>();

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.NeedsIdsQuery, CommandType.Text);
                dataReader = dataAccess.ExecuteReader();

                while (dataReader.Read())
                {
                    needidPoidDict.Add(dataReader[1].ToStringSafely(), dataReader[0].ToStringSafely());
                }
                log.Info("Total number of Need Ids: " + needidPoidDict.Count());

                GetNeedsData(needidPoidDict);
            }
            catch (Exception e)
            {
                log.Error("Caught!! Exception: " + e.InnerException.Message);
                throw;
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }
            log.Info("<-- SyncNeedsData");

        }

        private static void GetNeedsData(Dictionary<String, String> needidPoidDict)
        {
            log.Info("--> GetNeedsData");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            List<NeedsTableType> data = new List<NeedsTableType>();
            int count = 1;
            foreach (string key in needidPoidDict.Keys)
            {
                try
                {
                    client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                    //Setting timeout to max 150 seconds. Worst call historically has been 122 seconds
                    client.Timeout = TimeSpan.FromSeconds(150);
                    string uri = Settings.NeedsUrl.Replace("$",key);
                    client.BaseAddress = new Uri(uri);

                    response = client.GetAsync(uri).Result;
                    log.Info("(" + count++ +")" + " HttpResponseMessage statuscode for needId " + key + " : " + response.IsSuccessStatusCode);
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        var dataObjects = response.Content.ReadAsStringAsync().Result;
                        dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                        NeedsTableType temp = null;
                        for (int i = 0; i < x.value.Count; i++)
                        {
                            temp = new NeedsTableType();
                            //temp.NeedId = key;
                            temp.PoNumber = needidPoidDict[key];
                            temp.NeedsInfo = JsonConvert.DeserializeObject<NeedsInfo>(x.value[i].ToString());
                            if (temp.NeedsInfo.NeedId == key)
                                data.Add(temp);
                        }
                    }
                    else
                    {
                        log.Error("HttpResponseMessage statuscode for needId " + key + " : " + (int)response.StatusCode + ", Reason: " + response.ReasonPhrase);
                    }
                }
                catch (Exception e)
                {
                    log.Error("Failed to get needs info for NeedId: " + key + " : " + (int)response.StatusCode + ", Reason: " + response.ReasonPhrase);
                }
            }
            if(data.Count > 0)
                UpdateNeedsData(data);

            log.Info("<-- GetNeedsData");
        }

        private static void UpdateNeedsData(List<NeedsTableType> data)
        {
            log.Info("--> UpdateNeedsData");
            DataTable dt = new DataTable();
            dt.Columns.Add("[NeedId]", typeof(string));
            dt.Columns.Add("[Po]", typeof(string));
            dt.Columns.Add("[Type]", typeof(string));
            dt.Columns.Add("[Status]", typeof(string));
            dt.Columns.Add("[Pr.Number]", typeof(string));
            dt.Columns.Add("[Pr.Status]", typeof(string));
            dt.Columns.Add("[Pr.Due]", typeof(string));
            dt.Columns.Add("[PoLines.Po]", typeof(string));
            dt.Columns.Add("[PoLines.Line]", typeof(string));
            dt.Columns.Add("[PoLines.Type]", typeof(string));
            
            foreach (NeedsTableType d in data)
            {
                if (!d.NeedsInfo.Status.Equals("Cancelled"))
                {
                    if (d.NeedsInfo.PoLines.Length == 0)
                    {
                        dt.Rows.Add(
                           d.NeedsInfo.NeedId,
                           d.PoNumber,
                           d.NeedsInfo.Type,
                           d.NeedsInfo.Status,
                           d.NeedsInfo.Pr == null ? "" : d.NeedsInfo.Pr.Number,
                           d.NeedsInfo.Pr == null ? "" : d.NeedsInfo.Pr.Status,
                           d.NeedsInfo.Pr == null ? "" : d.NeedsInfo.Pr.Due,
                           "",
                           "",
                           ""
                       );
                    }
                    else {
                        for (int i = 0; i < d.NeedsInfo.PoLines.Length; i++)
                        {
                            dt.Rows.Add(
                                d.NeedsInfo.NeedId,
                                d.PoNumber,
                                d.NeedsInfo.Type,
                                d.NeedsInfo.Status,
                                d.NeedsInfo.Pr.Number,
                                d.NeedsInfo.Pr.Status,
                                d.NeedsInfo.Pr.Due,
                                d.NeedsInfo.PoLines == null ? "" : d.NeedsInfo.PoLines[i].Po,
                                d.NeedsInfo.PoLines == null ? "" : d.NeedsInfo.PoLines[i].Line,
                                d.NeedsInfo.PoLines == null ? "" : d.NeedsInfo.PoLines[i].Type
                            );
                        }
                    }
                }
                
            }
            log.Info("Total no. of records for NeedsInfo: " + dt.Rows.Count);

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, "[stage].[InsertUpdateNeedsInfo]");
                dataAccess.AddTableValueParameter("@inputTable", "[stage].[INeedsInfo]", dt);
                dataAccess.ExecuteReader();
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }
            log.Info("<-- UpdateNeedsData");
        }


        public void SyncOPOData()
        {
            log.Info("--> SyncOPOData");
            HttpClient client = null;
            try
            {
                //HttpClient client = new HttpClient(new HttpClientHandler { Credentials = new NetworkCredential("sys_msoac3", "GoC3ProjectsWinningTeam!") }); ;
                client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                client.BaseAddress = new Uri(Settings.EmsUrl);

                HttpResponseMessage response = client.GetAsync(Settings.EmsUrl).Result;
                log.Info("HttpResponseMessage statuscode: " + response.IsSuccessStatusCode);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    List<OrphanPO> data = new List<OrphanPO>();
                    for (int i = 0; i < x.value.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<OrphanPO>(x.value[i].ToString()));
                    }
                    log.Info("Total no. of records from EMS: " + data.Count);
                    if(data.Count > 0)
                        UpdateOrphanPOData(data);
                }
                else
                {
                    log.Error("HttpResponseMessage statuscode: " + (int)response.StatusCode + ", Reason: " + response.ReasonPhrase);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                throw;
            }
            finally {
                if(client.IsNotNull())
                    client.Dispose();
            }
            log.Info("<-- SyncOPOData");
        }

        private static void UpdateOrphanPOData(List<OrphanPO> data)
        {
            log.Info("--> UpdateOrphanPOData");
            DataTable dt = new DataTable();
            dt.Columns.Add("[Id]", typeof(string));
            dt.Columns.Add("[Status]", typeof(string));
            dt.Columns.Add("[EntityCode]", typeof(string));
            dt.Columns.Add("[BudgetArea]", typeof(string));
            dt.Columns.Add("[Site]", typeof(string));
            dt.Columns.Add("[Description]", typeof(string));
            dt.Columns.Add("[Type]", typeof(string));
            dt.Columns.Add("[Rtd]", typeof(string));
            dt.Columns.Add("[Std]", typeof(string));
            dt.Columns.Add("[ToolConfig.Ceid]", typeof(string));
            dt.Columns.Add("[ToolConfig.Process.Name]", typeof(string));
            dt.Columns.Add("[ToolConfig.Process.Group]", typeof(string));
            dt.Columns.Add("[Material.Mfg]", typeof(string));
            dt.Columns.Add("[PoLines.Po]", typeof(string));
            dt.Columns.Add("[PoLines.Line]", typeof(string));
            dt.Columns.Add("[PoLines.Type]", typeof(string));
            dt.Columns.Add("[PoLines.Cost]", typeof(string));
            dt.Columns.Add("[OI.Disposition]", typeof(string));
            dt.Columns.Add("[OI.CreatedAt]", typeof(string));
            dt.Columns.Add("[OI.CreatedBy]", typeof(string));
            dt.Columns.Add("[OI.UpdatedAt]", typeof(string));
            dt.Columns.Add("[OI.UpdatedBy]", typeof(string));
            dt.Columns.Add("[OI.ProposedNeed.Id]", typeof(string));
            dt.Columns.Add("[OI.ProposedNeed.Site]", typeof(string));
            dt.Columns.Add("[OI.ProposedNeed.EntityCode]", typeof(string));
            dt.Columns.Add("[OI.ProposedNeed.Ceid]", typeof(string));
            dt.Columns.Add("[OI.ProposedNeed.Process]", typeof(string));
            dt.Columns.Add("[OI.ProposedNeed.Rtd]", typeof(string));
            dt.Columns.Add("[OI.ProposedNeed.PrDue]", typeof(string));
            dt.Columns.Add("[OI.ProposeCancelDecisionEstimates.Cost]", typeof(string));
            dt.Columns.Add("[OI.ProposeCancelDecisionEstimates.NextCost]", typeof(string));
            dt.Columns.Add("[OI.ProposeCancelDecisionEstimates.NextDate]", typeof(string));
            dt.Columns.Add("[OI.ActualCancellationFeeInfo.Cost]", typeof(string));
            dt.Columns.Add("[OI.ActualCancellationFeeInfo.NextCost]", typeof(string));
            dt.Columns.Add("[OI.ActualCancellationFeeInfo.NextDate]", typeof(string));
            

            foreach (OrphanPO d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                for (int i = 0; i < d.PoLines.Length; i++) {
                        dt.Rows.Add(
                        d.Id,
                        d.Status,
                        d.EntityCode,
                        d.BudgetArea,
                        d.Site,
                        d.Description,
                        d.Type,
                        d.Rtd,
                        d.Std,
                        d.ToolConfig.Ceid == null ? "" : d.ToolConfig.Ceid,
                        d.ToolConfig.Process == null ? "" : d.ToolConfig.Process.Name,
                        d.ToolConfig.Process == null ? "" : d.ToolConfig.Process.Group,
                        d.Material.Mfg == null ? "" : d.Material.Mfg,
                        d.PoLines[i].Po,
                        d.PoLines[i].Line,
                        d.PoLines[i].Type,
                        d.PoLines[i].Cost,
                        d.OrphanInfo.Disposition == null ? "" : d.OrphanInfo.Disposition,
                        d.OrphanInfo.CreatedAt,
                        d.OrphanInfo.CreatedBy,
                        d.OrphanInfo.UpdatedAt,
                        d.OrphanInfo.UpdatedBy,
                        d.OrphanInfo.ProposedNeed == null ? "" : d.OrphanInfo.ProposedNeed.Id,
                        d.OrphanInfo.ProposedNeed == null ? "" : d.OrphanInfo.ProposedNeed.Site,
                        d.OrphanInfo.ProposedNeed == null ? "" : d.OrphanInfo.ProposedNeed.EntityCode,
                        d.OrphanInfo.ProposedNeed == null ? "" : d.OrphanInfo.ProposedNeed.Ceid,
                        d.OrphanInfo.ProposedNeed == null ? "" : d.OrphanInfo.ProposedNeed.Process,
                        d.OrphanInfo.ProposedNeed == null ? "" : d.OrphanInfo.ProposedNeed.Rtd,
                        d.OrphanInfo.ProposedNeed == null ? "" : d.OrphanInfo.ProposedNeed.PrDue,
                        d.OrphanInfo.ProposeCancelDecisionEstimates == null ? "" : d.OrphanInfo.ProposeCancelDecisionEstimates.Cost,
                        d.OrphanInfo.ProposeCancelDecisionEstimates == null ? "" : d.OrphanInfo.ProposeCancelDecisionEstimates.NextCost,
                        d.OrphanInfo.ProposeCancelDecisionEstimates == null ? "" : d.OrphanInfo.ProposeCancelDecisionEstimates.NextDate,
                        d.OrphanInfo.ActualCancellationFeeInfo == null ? "" : d.OrphanInfo.ActualCancellationFeeInfo.Cost,
                        d.OrphanInfo.ActualCancellationFeeInfo == null ? "" : d.OrphanInfo.ActualCancellationFeeInfo.NextCost,
                        d.OrphanInfo.ActualCancellationFeeInfo == null ? "" : d.OrphanInfo.ActualCancellationFeeInfo.NextDate
                    );
                }
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.EmsOrphanPOsSP);
                dataAccess.AddTableValueParameter("@inputTable", Settings.EmsOrphanPOsTableType, dt);
                dataAccess.ExecuteReader();
            }
            catch (Exception e)
            {
                if(e.InnerException.Message.IsNeitherNullNorEmpty())
                    log.Error("Caught!! SqlException: " + e.InnerException.Message);
                else
                    log.Error("Caught!! SqlException: " + e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }
            log.Info("<-- UpdateOrphanPOData");
        }
    }
}
