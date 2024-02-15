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
    public class GasChemGroupMorwConfigDataContext : IGasChemGroupMorwConfigDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public GasChemGroupMorwConfigDataContext()
        {
        }

        public void SyncGasChemGroupMorwConfigData()
        {
            log.Info("--> SyncGasChemGroupMorwConfigData");
            try
            {
                HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                string URL = Settings.GasChemGroupMorwConfigURL;
                client.BaseAddress = new Uri(URL);

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    GasChemGroupMorwConfigList data = new GasChemGroupMorwConfigList();
                    for (int i = 0; i < x.Data.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<GasChemGroupMorwConfig>(x.Data[i].ToString()));
                    }
                    log.Info("SyncGasChemGroupMorwConfigData: Total number of records fetched from GasChemGroupMorwConfig: " + data.Count);
                    UpdateGasChemGroupMorwConfigDataToDb(data);
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
            log.Info("<-- SyncGasChemGroupMorwConfigData");

        }

        private void UpdateGasChemGroupMorwConfigDataToDb(GasChemGroupMorwConfigList data)
        {
            log.Info("--> UpdateGasChemGroupMorwConfigDataToDb");

            DataTable dt = new DataTable();
            dt.Columns.Add("[Action]", typeof(string));
            dt.Columns.Add("[Process]", typeof(string));
            dt.Columns.Add("[EquipmentCeid]", typeof(string));
            dt.Columns.Add("[GasChemGroup]", typeof(string));
            dt.Columns.Add("[MorwCons]", typeof(string));
            dt.Columns.Add("[Errors]", typeof(string));

            foreach (GasChemGroupMorwConfig d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    d.Action,
                    d.Process,
                    d.EquipmentCeid,
                    d.GasChemGroup,
                    d.MorwCons,
                    d.Errors
                );
            }

            using (SqlConnection connection = new SqlConnection(Settings.IntegrationDbConnString))
            {
                using (SqlCommand command = new SqlCommand(Settings.GasChemGroupMorwConfigSP, connection))
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
                        log.Error("UpdateGasChemGroupMorwConfigDataToDb: Caught!! SqlException: " + e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            log.Info("<-- UpdateGasChemGroupMorwConfigDataToDb");

        }
    }
}
