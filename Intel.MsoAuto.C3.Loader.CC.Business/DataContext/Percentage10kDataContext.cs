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
    public class Percentage10kDataContext : IPercentage10kDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Percentage10kDataContext()
        {
            
        }

        public void SyncPct10kData()
        {
            log.Info("--> SyncPct10kData");
            try
            {
                HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                string URL = Settings.Pct10kURL;
                client.BaseAddress = new Uri(URL);

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    dynamic x = JsonConvert.DeserializeObject<dynamic>(dataObjects);
                    Percentage10KList data = new Percentage10KList();
                    for (int i = 0; i < x.Data.Count; i++)
                    {
                        data.Add(JsonConvert.DeserializeObject<Percentage10k>(x.Data[i].ToString()));
                    }
                    log.Info("SyncPct10kData: Total number of records fetched from Pct10kData: " + data.Count);
                    UpdatePct10kDataToDb(data);
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
            log.Info("<-- SyncPct10kData ");
        }

        private void UpdatePct10kDataToDb(Percentage10KList data)
        {
            log.Info("--> UpdatePct10kDataToDb");

            DataTable dt = new DataTable();
            dt.Columns.Add("[WorkWeek]", typeof(string));
            dt.Columns.Add("[Facility]", typeof(string));
            dt.Columns.Add("[TechNode]", typeof(string));
            dt.Columns.Add("[OrgArea]", typeof(string));
            dt.Columns.Add("[OrgUnit]", typeof(string));
            dt.Columns.Add("[ParentCEID]", typeof(string));
            dt.Columns.Add("[Commodity]", typeof(string));
            dt.Columns.Add("[VariablePercent]", typeof(string));
            dt.Columns.Add("[ToolCount]", typeof(string));
            dt.Columns.Add("[TenKToolCount]", typeof(string));
            dt.Columns.Add("[ScaledWSE]", typeof(string));
            dt.Columns.Add("[Percentof10K]", typeof(string));
            dt.Columns.Add("[LastUpdatedDate]", typeof(string));

            foreach (Percentage10k d in data)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    d.WorkWeek,
                    d.Facility,
                    d.TechNode,
                    d.OrgArea,
                    d.OrgUnit,
                    d.ParentCEID,
                    d.Commodity,
                    d.VariablePercent,
                    d.ToolCount,
                    d.TenKToolCount,
                    d.ScaledWSE,
                    d.Percentof10K,
                    d.LastUpdatedDate
                );
            }

            using (SqlConnection connection = new SqlConnection(Settings.IntegrationDbConnString))
            {
                using (SqlCommand command = new SqlCommand(Settings.Pct10kSP, connection))
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
                        log.Error("UpdatePct10kDataToDb: Caught!! SqlException: " + e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            log.Info("<-- UpdatePct10kDataToDb");

        }
    }
}
