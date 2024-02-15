using Intel.MsoAuto.C3.Loader.CC.Business.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Intel.MsoAuto.Shared;
using System.Reflection;
using log4net;
using log4net.Config;
using Intel.MsoAuto.C3.Loader.CC.Business.Core;

namespace Intel.MsoAuto.C3.Loader.CC.Business.DataContext
{
    public class EquipmentCeidWseDataContext: IEquipmentCeidWseDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EquipmentCeidWseDataContext()
        {
        }

        public void SyncEquipmentCeidWseData()
        {
            log.Info("--> SyncEquipmentCeidWseData");
            try
            {
                HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                string URL = Settings.EquipmentCeidWseURL;
                client.BaseAddress = new Uri(URL);

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    EquipmentCeidWseList data = new EquipmentCeidWseList();
                    for (int i = 0; i < x.Data.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<EquipmentCeidWse>(x.Data[i].ToString()));
                    }
                    log.Info("SyncEquipmentCeidWseData: Total number of records fetched from EquipmentCeidWse: " + data.Count);
                    UpdateEquipmentCeidWseDataToDb(data);
                }
                else
                {
                    log.Info((int)response.StatusCode + " (" + response.ReasonPhrase + ")");
                }
                client.Dispose();
            }
            catch(Exception e)
            {
                log.Error(e);
                throw;
            }
            log.Info("<-- SyncEquipmentCeidWseData ");
        }

        private void UpdateEquipmentCeidWseDataToDb(EquipmentCeidWseList data)
        {
            log.Info("--> UpdateEquipmentCeidWseDataToDb");

            DataTable dt = new DataTable();
            dt.Columns.Add("[WorkWeek]", typeof(string));
            dt.Columns.Add("[Facility]", typeof(string));
            dt.Columns.Add("[TechNode]", typeof(string));
            dt.Columns.Add("[Process]", typeof(string));
            dt.Columns.Add("[FunctionalArea]", typeof(string));
            dt.Columns.Add("[OrgArea]", typeof(string));
            dt.Columns.Add("[OrgUnit]", typeof(string));
            dt.Columns.Add("[ParentCEID]", typeof(string));
            dt.Columns.Add("[EquipmentCEID]", typeof(string));
            dt.Columns.Add("[IsNoOuts]", typeof(string));
            dt.Columns.Add("[SourceOuts]", typeof(string));
            dt.Columns.Add("[SourceOutsPrime]", typeof(string));
            dt.Columns.Add("[Outs]", typeof(string));
            dt.Columns.Add("[OutsPrime]", typeof(string));
            dt.Columns.Add("[WSE]", typeof(string));
            dt.Columns.Add("[WSEPrime]", typeof(string));
            dt.Columns.Add("[LastUpdatedDate]", typeof(string));

            foreach (EquipmentCeidWse d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    d.WorkWeek,
                    d.Facility,
                    d.TechNode,
                    d.Process,
                    d.FunctionalArea,
                    d.OrgArea,
                    d.OrgUnit,
                    d.ParentCEID,
                    d.EquipmentCEID,
                    d.IsNoOuts,
                    d.SourceOuts,
                    d.SourceOutsPrime,
                    d.Outs,
                    d.OutsPrime,
                    d.WSE,
                    d.WSEPrime,
                    d.LastUpdatedDate
                );
            }

            using (SqlConnection connection = new SqlConnection(Settings.IntegrationDbConnString))
            {
                using (SqlCommand command = new SqlCommand(Settings.EquipmentCeidWseSP, connection))
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
                        log.Error("UpdateEquipmentCeidWseDataToDb: Caught!! SqlException: " + e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            log.Info("<-- UpdateEquipmentCeidWseDataToDb");

        }
    }
}
