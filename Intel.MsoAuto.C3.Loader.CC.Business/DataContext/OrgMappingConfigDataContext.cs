using Intel.MsoAuto.C3.Loader.CC.Business.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using Intel.MsoAuto.Shared;
using log4net;
using log4net.Config;
using System.Reflection;
using Intel.MsoAuto.C3.Loader.CC.Business.Core;

namespace Intel.MsoAuto.C3.Loader.CC.Business.DataContext
{
    public class OrgMappingConfigDataContext : IOrgMappingConfigDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public OrgMappingConfigDataContext()
        {
        }

        public void SyncOrgMappingConfigData()
        {
            log.Info("--> SyncOrgMappingConfigData");
            try
            {
                HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                string URL = Settings.OrgMappingConfigURL;
                client.BaseAddress = new Uri(URL);

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    OrgMappingConfigList data = new OrgMappingConfigList();
                    for (int i = 0; i < x.Data.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<OrgMappingConfig>(x.Data[i].ToString()));
                    }
                    log.Info("SyncOrgMappingConfigData: Total number of records fetched from OrgMappingConfig: " + data.Count);
                    UpdateOrgMappingConfigDataToDb(data);
                }
                else
                {
                    log.Info((int)response.StatusCode + " (" + response.ReasonPhrase + ")");
                }
                client.Dispose();
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
            log.Info("<-- SyncOrgMappingConfigData");
        }

        private void UpdateOrgMappingConfigDataToDb(OrgMappingConfigList data)
        {
            log.Info("--> UpdateOrgMappingConfigDataToDb");

            DataTable dt = new DataTable();
            dt.Columns.Add("[Action]", typeof(string));
            dt.Columns.Add("[Facility]", typeof(string));
            dt.Columns.Add("[TechNode]", typeof(string));
            dt.Columns.Add("[OrgArea]", typeof(string));
            dt.Columns.Add("[OrgUnit]", typeof(string));
            dt.Columns.Add("[ParentCeid]", typeof(string));
            dt.Columns.Add("[Errors]", typeof(string));

            foreach (OrgMappingConfig d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    d.Action,
                    d.Facility,
                    d.TechNode,
                    d.OrgArea,
                    d.OrgUnit,
                    d.ParentCeid,
                    d.Errors
                );
            }

            using (SqlConnection connection = new SqlConnection(Settings.IntegrationDbConnString))
            {
                using (SqlCommand command = new SqlCommand(Settings.OrgMappingConfigSP, connection))
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@inputTable", dt);
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        log.Error("UpdateOrgMappingConfigDataToDb: Caught!! SqlException: " + e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            log.Info("<-- UpdateOrgMappingConfigDataToDb");

        }
    }
}
