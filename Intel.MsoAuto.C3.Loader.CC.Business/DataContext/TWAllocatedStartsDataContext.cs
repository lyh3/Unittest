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
    public class TWAllocatedStartsDataContext : ITWAllocatedStartsDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TWAllocatedStartsDataContext()
        {
        }

        public void SyncTWAllocatedStartsData()
        {
            log.Info("--> SyncTWAllocatedStartsData");
            try
            {
                HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                string URL = Settings.TWAllocatedStartsURL;
                client.BaseAddress = new Uri(URL);

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    TWAllocatedStartsList data = new TWAllocatedStartsList();
                    for (int i = 0; i < x.Data.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<TWAllocatedStarts>(x.Data[i].ToString()));
                    }
                    log.Info("SyncTWAllocatedStartsData: Total number of records fetched from TWAllocatedStarts: " + data.Count);
                    UpdateTWAllocatedStartsDataToDb(data);
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
                throw;
            }
            log.Info("<-- SyncTWAllocatedStartsData");
        }

        private void UpdateTWAllocatedStartsDataToDb(TWAllocatedStartsList data)
        {
            log.Info("--> UpdateTWAllocatedStartsDataToDb");

            DataTable dt = new DataTable();
            dt.Columns.Add("[WorkWeek]", typeof(string));
            dt.Columns.Add("[TechNode]", typeof(string));
            dt.Columns.Add("[Process]", typeof(string));
            dt.Columns.Add("[ParentCeid]", typeof(string));
            dt.Columns.Add("[EquipCeid]", typeof(string));
            dt.Columns.Add("[Route]", typeof(string));
            dt.Columns.Add("[RouteDesc]", typeof(string));

            foreach (TWAllocatedStarts d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    d.WorkWeek,
                    d.TechNode,
                    d.Process,
                    d.ParentCeid,
                    d.EquipCeid,
                    d.Route,
                    d.RouteDesc
                );
            }

            using (SqlConnection connection = new SqlConnection(Settings.IntegrationDbConnString))
            {
                using (SqlCommand command = new SqlCommand(Settings.TWAllocatedStartsSP, connection))
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
                        log.Error("UpdateTWAllocatedStartsDataToDb: Caught!! SqlException: " + e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            log.Info("<-- UpdateTWAllocatedStartsDataToDb");

        }
    }
}
