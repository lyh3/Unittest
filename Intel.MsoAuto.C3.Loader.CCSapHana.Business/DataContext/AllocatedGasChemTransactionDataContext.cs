using Intel.MsoAuto.C3.Loader.CCSapHana.Business.Entity;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.DataContext
{
    internal class AllocatedGasChemTransactionDataContext : IAllocatedGasChemTransactionDataContext
    {

        public AllocatedGasChemTransactionDataContext()
        {

        }

        public static string GetHanaValue(HanaDataReader reader, string colName)
        {
            return reader[colName] == null ? null : reader[colName].ToString();
        }

        public bool SyncAllocatedGasChemTransactionData()
        {
            try
            {
                var builder = new ConfigurationBuilder()                    
                   .AddJsonFile("appSettings.json", false, true);
                var config = builder.Build();

                string integrationDbConnString = config.GetSection("IntegrationDbOptions:ConnectionString").Value;
                string integrationSP = config.GetSection("IntegrationDbOptions:GasChemTransactionSP").Value;
                string GasChemDbConnString = config.GetSection("SapDbOptions:ConnectionString").Value;
                string GasChemDbView = config.GetSection("SapDbOptions:AllocatedGasChemTransaction").Value;
                string GasChemQuery = config.GetSection("SapDbOptions:AllocatedGasChemQuery").Value;

                using (HanaConnection conn = new HanaConnection(GasChemDbConnString))
                {
                    Console.WriteLine("Connecting...");
                    conn.Open();
                    Console.WriteLine("Connected");

                    //HanaCommand cmd = new HanaCommand($"SELECT * FROM {GasChemDbView}", conn);
                    HanaCommand cmd = new HanaCommand(GasChemQuery, conn);
                    cmd.CommandType = CommandType.Text;
                    HanaDataReader reader = cmd.ExecuteReader();
                    AllocatedGasChemTransactionList agctList = new AllocatedGasChemTransactionList();
                    while (reader.Read())
                    {
                        AllocatedGasChemTransaction agct = new AllocatedGasChemTransaction();
                        
                        agct.FacilityNm = GetHanaValue(reader, "FacilityNm");
                        agct.TechnologyNodeNm = GetHanaValue(reader, "TechnologyNodeNm");
                        agct.ProcessNm = GetHanaValue(reader, "ProcessNm");
                        agct.FunctionalAreaNm = GetHanaValue(reader, "FunctionalAreaNm");
                        agct.OrganizationAreaNm = GetHanaValue(reader, "OrganizationAreaNm");
                        agct.OrganizationUnitNm = GetHanaValue(reader, "OrganizationUnitNm");
                        agct.ParentCapitalEquipmentId = GetHanaValue(reader, "ParentCapitalEquipmentId");
                        agct.CapitalEquipmentId = GetHanaValue(reader, "CapitalEquipmentId");
                        agct.GasChemGroupNm = GetHanaValue(reader, "GasChemGroupNm");
                        agct.GasChemNm = GetHanaValue(reader, "GasChemNm");
                        agct.GasChemDsc = GetHanaValue(reader, "GasChemDsc");
                        agct.SupplierNm = GetHanaValue(reader, "SupplierNm");
                        agct.WorkWeekNbr = GetHanaValue(reader, "WorkWeekNbr");
                        agct.UniqueEquipmentId = GetHanaValue(reader, "UniqueEquipmentId");
                        agct.AllocatedGasChemQty = GetHanaValue(reader, "AllocatedGasChemQty");
                        agct.SupplierId = GetHanaValue(reader, "SupplierId");
                        agct.TransactionDtm = GetHanaValue(reader, "TransactionDtm");
                        agct.LastUpdatedDtm = GetHanaValue(reader, "LastUpdatedDtm");
                        agct.AsOfSourceDtm = GetHanaValue(reader, "AsOfSourceDtm");
                        agct.AsOfTargetDtm = GetHanaValue(reader, "AsOfTargetDtm");

                        agctList.Add(agct);
                    }

                    DataTable dt = new DataTable();

                    dt.Columns.Add("[FacilityNm]", typeof(string));
                    dt.Columns.Add("[TechnologyNodeNm]", typeof(string));
                    dt.Columns.Add("[ProcessNm]", typeof(string));
                    dt.Columns.Add("[FunctionalAreaNm]", typeof(string));
                    dt.Columns.Add("[OrganizationAreaNm]", typeof(string));
                    dt.Columns.Add("[OrganizationUnitNm]", typeof(string));
                    dt.Columns.Add("[ParentCapitalEquipmentId]", typeof(string));
                    dt.Columns.Add("[CapitalEquipmentId]", typeof(string));
                    dt.Columns.Add("[GasChemGroupNm]", typeof(string));
                    dt.Columns.Add("[GasChemNm]", typeof(string));
                    dt.Columns.Add("[GasChemDsc]", typeof(string));
                    dt.Columns.Add("[SupplierNm]", typeof(string));
                    dt.Columns.Add("[WorkWeekNbr]", typeof(string));
                    dt.Columns.Add("[UniqueEquipmentId]", typeof(string));
                    dt.Columns.Add("[AllocatedGasChemQty]", typeof(string));
                    dt.Columns.Add("[SupplierId]", typeof(string));
                    dt.Columns.Add("[TransactionDtm]", typeof(string));
                    dt.Columns.Add("[LastUpdatedDtm]", typeof(string));
                    dt.Columns.Add("[AsOfSourceDtm]", typeof(string));
                    dt.Columns.Add("[AsOfTargetDtm]", typeof(string));

                    foreach (AllocatedGasChemTransaction agct in agctList)
                    //for (int i= 0;i<10;i++)
                    {
                        //OrphanedPO opo = orphanedPOList.ElementAt(i);
                        dt.Rows.Add(
                            agct.FacilityNm,
                            agct.TechnologyNodeNm,
                            agct.ProcessNm,
                            agct.FunctionalAreaNm,
                            agct.OrganizationAreaNm,
                            agct.OrganizationUnitNm,
                            agct.ParentCapitalEquipmentId,
                            agct.CapitalEquipmentId,
                            agct.GasChemGroupNm,
                            agct.GasChemNm,
                            agct.GasChemDsc,
                            agct.SupplierNm,
                            agct.WorkWeekNbr,
                            agct.UniqueEquipmentId,
                            agct.AllocatedGasChemQty,
                            agct.SupplierId,
                            agct.TransactionDtm,
                            agct.LastUpdatedDtm,
                            agct.AsOfSourceDtm,
                            agct.AsOfTargetDtm
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
