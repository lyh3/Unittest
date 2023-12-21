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
    public class ScaledSpendingAnalysisExportDataContext : IScaledSpendingAnalysisExportDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ScaledSpendingAnalysisExportDataContext()
        {
        }

        public void SyncScaledSpendingAnalysisExportData()
        {
            log.Info("--> SyncScaledSpendingAnalysisExportData");
            try
            {
                HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                string URL = Settings.ScaledSpendingAnalysisExportURL;
                client.BaseAddress = new Uri(URL);

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    ScaledSpendingAnalysisExportList data = new ScaledSpendingAnalysisExportList();
                    for (int i = 0; i < x.Data.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<ScaledSpendingAnalysisExport>(x.Data[i].ToString()));
                    }
                    log.Info("SyncScaledSpendingAnalysisExportData: Total number of records fetched from ScaledSpendingAnalysisExport: " + data.Count);
                    UpdateScaledSpendingAnalysisExportDataToDb(data);
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
            log.Info("<-- SyncScaledSpendingAnalysisExportData");
        }

        private void UpdateScaledSpendingAnalysisExportDataToDb(ScaledSpendingAnalysisExportList data)
        {
            log.Info("--> UpdateScaledSpendingAnalysisExportDataToDb");

            DataTable dt = new DataTable();
            dt.Columns.Add("[WorkWeek]", typeof(string));
            dt.Columns.Add("[Facility]", typeof(string));
            dt.Columns.Add("[TechNode]", typeof(string));
            dt.Columns.Add("[ParentCEID]", typeof(string));
            dt.Columns.Add("[Commodity]", typeof(string));
            dt.Columns.Add("[IPN]", typeof(string));
            dt.Columns.Add("[Scaled13WeekConsumption]", typeof(string));

            foreach (ScaledSpendingAnalysisExport d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    d.WorkWeek,
                    d.Facility,
                    d.TechNode,
                    d.ParentCEID,
                    d.Commodity,
                    d.IPN,
                    d.Scaled13WeekConsumption
                );
            }

            using (SqlConnection connection = new SqlConnection(Settings.IntegrationDbConnString))
            {
                using (SqlCommand command = new SqlCommand(Settings.ScaledSpendingAnalysisExportSP, connection))
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
                        log.Error("UpdateScaledSpendingAnalysisExportDataToDb: Caught!! SqlException: " + e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            log.Info("<-- UpdateScaledSpendingAnalysisExportDataToDb");

        }
    }

}
