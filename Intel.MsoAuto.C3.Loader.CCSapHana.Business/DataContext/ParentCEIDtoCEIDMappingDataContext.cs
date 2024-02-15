using Intel.MsoAuto.C3.Loader.CCSapHana.Business.Entity;
using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.DataContext
{
    internal class ParentCEIDtoCEIDMappingDataContext : IParentCEIDtoCEIDMappingDataContext
    {

        public ParentCEIDtoCEIDMappingDataContext()
        {

        }

        public static string GetHanaValue(HanaDataReader reader, string colName)
        {
            return reader[colName] == null ? null : reader[colName].ToString();
        }

        public bool SyncParentCEIDtoCEIDMappingData()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                   .AddJsonFile("appSettings.json", false, true);
                var config = builder.Build();

                string integrationDbConnString = config.GetSection("IntegrationDbOptions:ConnectionString").Value;
                string integrationSP = config.GetSection("IntegrationDbOptions:ParentCEIDMappingSP").Value;
                string parentCEIDDbConnString = config.GetSection("SapDbOptions:ConnectionString").Value;
                string parentCEIDDbView = config.GetSection("SapDbOptions:ParentCEIDtoCEIDMapping").Value;

                using (HanaConnection conn = new HanaConnection(parentCEIDDbConnString))
                {
                    Console.WriteLine("Connecting...");
                    conn.Open();
                    Console.WriteLine("Connected");

                    HanaCommand cmd = new HanaCommand($"SELECT * FROM {parentCEIDDbView}", conn);
                    cmd.CommandType = CommandType.Text;
                    HanaDataReader reader = cmd.ExecuteReader();
                    ParentCEIDtoCEIDMappingList pceidList = new ParentCEIDtoCEIDMappingList();
                    while (reader.Read())
                    {
                        ParentCEIDtoCEIDMapping pceid = new ParentCEIDtoCEIDMapping();

                        pceid.TechnologyNodeNm = GetHanaValue(reader, "TechnologyNodeNm");
                        pceid.CapitalEquipmentIdentifier = GetHanaValue(reader, "CapitalEquipmentIdentifier");
                        pceid.ParentCapitalEquipmentIdentifier = GetHanaValue(reader, "ParentCapitalEquipmentIdentifier");
                        pceid.ActiveInd = GetHanaValue(reader, "ActiveInd");
                        pceid.AsOfSourceTs = GetHanaValue(reader, "AsOfSourceTs");
                        pceid.AsOfTargetTs = GetHanaValue(reader, "AsOfTargetTs");
                        pceidList.Add(pceid);
                    }

                    DataTable dt = new DataTable();

                    dt.Columns.Add("[TechnologyNodeNm]", typeof(string));
                    dt.Columns.Add("[CapitalEquipmentIdentifier]", typeof(string));
                    dt.Columns.Add("[ParentCapitalEquipmentIdentifier]", typeof(string));
                    dt.Columns.Add("[ActiveInd]", typeof(string));
                    dt.Columns.Add("[AsOfSourceTs]", typeof(string));
                    dt.Columns.Add("[AsOfTargetTs]", typeof(string));
                   
                    foreach (ParentCEIDtoCEIDMapping pceid in pceidList)
                    //for (int i= 0;i<10;i++)
                    {
                        //OrphanedPO opo = orphanedPOList.ElementAt(i);
                        dt.Rows.Add(
                            pceid.TechnologyNodeNm,
                            pceid.CapitalEquipmentIdentifier,
                            pceid.ParentCapitalEquipmentIdentifier,
                            pceid.ActiveInd,
                            pceid.AsOfSourceTs,
                            pceid.AsOfTargetTs
                        );
                    }

                    using (SqlConnection connection = new SqlConnection(integrationDbConnString))
                    {
                        using (SqlCommand command = new SqlCommand(integrationSP, connection))
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
                                Console.WriteLine("Caught!! SqlException: " + e.Message);
                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }

                    Console.WriteLine("Disconnecting...");
                    conn.Close();
                    Console.WriteLine("Disconnected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
