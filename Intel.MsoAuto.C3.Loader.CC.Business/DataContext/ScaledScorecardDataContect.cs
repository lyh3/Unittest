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

    public class ScaledScorecardDataContect : IScaledScorecardDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ScaledScorecardDataContect()
        {
        }

        public void SyncScaledScorecardData()
        {
            log.Info("--> SyncScaledScorecardData");
            try
            {
                HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                string URL = Settings.ScaledScorecardURL;
                client.BaseAddress = new Uri(URL);

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    ScaledScorecardList data = new ScaledScorecardList();
                    for (int i = 0; i < x.Data.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<ScaledScorecard>(x.Data[i].ToString()));
                    }
                    log.Info("SyncScaledScorecardData: Total number of records fetched from ScaledScorecardData: " + data.Count);
                    UpdateScaledScorecardDataToDb(data);
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
            log.Info("<-- SyncScaledScorecardData");
        }

        private void UpdateScaledScorecardDataToDb(ScaledScorecardList data)
        {
            log.Info("--> UpdateScaledScorecardDataToDb");

            DataTable dt = new DataTable();
            dt.Columns.Add("[WorkWeek]", typeof(string));
            dt.Columns.Add("[Facility]", typeof(string));
            dt.Columns.Add("[TechNode]", typeof(string));
            dt.Columns.Add("[ParentCEID]", typeof(string));
            dt.Columns.Add("[Commodity]", typeof(string));
            dt.Columns.Add("[Scaled13WeekConsumption]", typeof(string));

            foreach (ScaledScorecard d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    d.WorkWeek,
                    d.Facility,
                    d.TechNode,
                    d.ParentCEID,
                    d.Commodity,
                    d.Scaled13WeekConsumption
                );
            }

            using (SqlConnection connection = new SqlConnection(Settings.IntegrationDbConnString))
            {
                using (SqlCommand command = new SqlCommand(Settings.ScaledScorecardSP, connection))
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
                        log.Error("UpdateScaledScorecardDataToDb: Caught!! SqlException: " + e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            log.Info("<-- UpdateScaledScorecardDataToDb");

        }
    }
}
