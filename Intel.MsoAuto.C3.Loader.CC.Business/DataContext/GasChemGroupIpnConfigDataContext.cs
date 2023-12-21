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
    public class GasChemGroupIpnConfigDataContext : IGasChemGroupIpnConfigDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public GasChemGroupIpnConfigDataContext()
        {
        }

        public void SyncGasChemGroupIpnConfigData()
        {
            log.Info("--> SyncGasChemGroupIpnConfigData");
            try
            {
                HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                string URL = Settings.GasChemGroupIpnConfigURL;
                client.BaseAddress = new Uri(URL);

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    GasChemGroupIpnConfigList data = new GasChemGroupIpnConfigList();
                    for (int i = 0; i < x.Data.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<GasChemGroupIpnConfig>(x.Data[i].ToString()));
                    }
                    log.Info("SyncGasChemGroupIpnConfigData: Total number of records fetched from GasChemGroupIpnConfig: " + data.Count);

                    UpdateGasChemGroupIpnConfigToDb(data);
                }
                else
                {
                    Console.WriteLine((int)response.StatusCode + " (" + response.ReasonPhrase + ")");
                }
                client.Dispose();
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
            log.Info("<-- SyncGasChemGroupIpnConfigData");

        }

        private void UpdateGasChemGroupIpnConfigToDb(GasChemGroupIpnConfigList data)
        {
            log.Info("--> UpdateGasChemGroupIpnConfigToDb");

            DataTable dt = new DataTable();
            dt.Columns.Add("[Action]", typeof(string));
            dt.Columns.Add("[IPN]", typeof(string));
            dt.Columns.Add("[IPNDesc]", typeof(string));
            dt.Columns.Add("[GasChemGrp]", typeof(string));
            dt.Columns.Add("[Errors]", typeof(string));
           

            foreach (GasChemGroupIpnConfig d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    d.Action,
                    d.IPN,
                    d.IPNDesc,
                    d.GasChemGrp,
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
                        log.Error("UpdateGasChemGroupIpnConfigToDb: Caught!! SqlException: " + e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            log.Info("<-- UpdateGasChemGroupIpnConfigToDb");

        }
    }
}

