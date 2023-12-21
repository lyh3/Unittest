using Intel.MsoAuto.C3.Loader.CC.Business.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using Intel.MsoAuto.Shared;
using log4net.Config;
using System.Reflection;
using log4net;
using Intel.MsoAuto.C3.Loader.CC.Business.Core;

namespace Intel.MsoAuto.C3.Loader.CC.Business.DataContext
{
    public class FsscToolCountConfigDataContext: IFsscToolCountConfigDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FsscToolCountConfigDataContext()
        {    
        }

        public void SyncFsscToolCountConfigData()
        {
            log.Info("--> SyncFsscToolCountConfigData");
            try
            {
                HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                string URL = Settings.FsscToolCountConfigurationURL;
                log.Info("FsscToolCountConfigDataContext: SyncFsscToolCountConfigData: URL = " + URL);
                client.BaseAddress = new Uri(URL);

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    log.Info("SyncFsscToolCountConfigData: response.IsSuccessStatusCode = true");
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    List<FsscToolCountConfig> data = new List<FsscToolCountConfig>();
                    for (int i = 0; i < x.Data.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<FsscToolCountConfig>(x.Data[i].ToString()));
                    }
                    log.Info("SyncFsscToolCountConfigData: Total number of records fetched from FsscToolCountConfig: " + data.Count);

                    UpdateFsscToolCountConfigDataToDb(data);
                }
                else
                {
                    log.Info("SyncFsscToolCountConfigData: " + (int)response.StatusCode + " (" + response.ReasonPhrase + ")");
                }
                client.Dispose();
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
            log.Info("<-- SyncFsscToolCountConfigData");
        }

        private void UpdateFsscToolCountConfigDataToDb(List<FsscToolCountConfig> data)
        {
            log.Info("--> UpdateFsscToolCountConfigDataToDb");
            DataTable dt = new DataTable();
            dt.Columns.Add("[Action]", typeof(string));
            dt.Columns.Add("[WorkWeek]", typeof(string));
            dt.Columns.Add("[Facility]", typeof(string));
            dt.Columns.Add("[Building]", typeof(string));
            dt.Columns.Add("[Process]", typeof(string));
            dt.Columns.Add("[EquipmentCEID]", typeof(string));
            dt.Columns.Add("[ProductionToolCount]", typeof(string));
            dt.Columns.Add("[TotalToolCount]", typeof(string));
            dt.Columns.Add("[Comment]", typeof(string));
            dt.Columns.Add("[Errors]", typeof(string));

            foreach (FsscToolCountConfig d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    d.Action,
                    d.WorkWeek,
                    d.Facility,
                    d.Building,
                    d.Process,
                    d.EquipmentCEID,
                    d.ProductionToolCount,
                    d.TotalToolCount,
                    d.Comment,
                    d.Errors
                );
            }

            using (SqlConnection connection = new SqlConnection(Settings.IntegrationDbConnString))
            {
                using (SqlCommand command = new SqlCommand(Settings.FsscToolCountConfigurationSP, connection))
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
                        log.Error("UpdateFsscToolCountConfigDataToDb: Caught!! SqlException: " + e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            log.Info("<-- UpdateFsscToolCountConfigDataToDb");
        }
    }
}
