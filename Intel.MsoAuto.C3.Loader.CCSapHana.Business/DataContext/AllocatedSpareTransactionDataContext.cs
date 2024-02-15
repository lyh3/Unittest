using Intel.MsoAuto.C3.Loader.CCSapHana.Business.Entity;
using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Reflection.PortableExecutable;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.DataContext
{
    internal class AllocatedSpareTransactionDataContext : IAllocatedSpareTransactionDataContext
    {

        public AllocatedSpareTransactionDataContext()
        {

        }

        public static string GetHanaValue(HanaDataReader reader, string colName)
        { 
            return reader[colName] == null ? null : reader[colName].ToString();
        }

        public bool SyncAllocatedSpareTransactionData()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                   .AddJsonFile("appSettings.json", false, true);
                var config = builder.Build();

                string integrationDbConnString = config.GetSection("IntegrationDbOptions:ConnectionString").Value;
                string integrationSP = config.GetSection("IntegrationDbOptions:SpareTransactionSP").Value;
                string SpareTransactionDbConnString = config.GetSection("SapDbOptions:ConnectionString").Value;
                string SpareTransactionDbView = config.GetSection("SapDbOptions:AllocatedSpareTransaction").Value;
                string SpareTransactionQuery = config.GetSection("SapDbOptions:AllocatedSpareQuery").Value;

                using (HanaConnection conn = new HanaConnection(SpareTransactionDbConnString))
                {
                    Console.WriteLine("Connecting...");
                    conn.Open();
                    Console.WriteLine("Connected");

                    //HanaCommand cmd = new HanaCommand($"SELECT * FROM {SpareTransactionDbView}", conn);
                    HanaCommand cmd = new HanaCommand(SpareTransactionQuery, conn);
                    cmd.CommandType = CommandType.Text;
                    HanaDataReader reader = cmd.ExecuteReader();
                    AllocatedSpareTransactionList astList = new AllocatedSpareTransactionList();
                    while (reader.Read())
                    {
                        AllocatedSpareTransaction ast = new AllocatedSpareTransaction();
                        ast.FacilityName = GetHanaValue(reader, "FacilityName");
                        ast.TechnologyNodeName = GetHanaValue(reader, "TechnologyNodeName");
                        ast.ProcessName = GetHanaValue(reader, "ProcessName");
                        ast.FunctionalAreaName = GetHanaValue(reader, "FunctionalAreaName");
                        ast.OrganizationAreaName = GetHanaValue(reader, "OrganizationAreaName");
                        ast.OrganizationUnitName = GetHanaValue(reader, "OrganizationUnitName");
                        ast.ParentCapitalEquipmentIdentifier = GetHanaValue(reader, "ParentCapitalEquipmentIdentifier");
                        ast.CapitalEquipmentIdentifier = GetHanaValue(reader, "CapitalEquipmentIdentifier");
                        ast.SparePartNumber = GetHanaValue(reader, "SparePartNumber");
                        ast.SparePartDescription = GetHanaValue(reader, "SparePartDescription");
                        ast.SupplierIdentifier = GetHanaValue(reader, "SupplierIdentifier");
                        ast.SupplierName = GetHanaValue(reader, "SupplierName");
                        ast.SupplierPartNumber = GetHanaValue(reader, "SupplierPartNumber");
                        ast.WorkWeekNumber = GetHanaValue(reader, "WorkWeekNumber");
                        ast.UniqueEquipmentIdentifier = GetHanaValue(reader, "UniqueEquipmentIdentifier");
                        ast.ConsumedQuantity = GetHanaValue(reader, "ConsumedQuantity");
                        ast.ConsumptionAmount = GetHanaValue(reader, "ConsumptionAmount");
                        ast.CostToIntelAmount = GetHanaValue(reader, "CostToIntelAmount");
                        ast.TransactionDateTime = GetHanaValue(reader, "TransactionDateTime");
                        ast.LastUpdatedDateTime = GetHanaValue(reader, "LastUpdatedDateTime");
                        ast.AsOfSourceDtm = GetHanaValue(reader, "AsOfSourceDtm");
                        ast.AsOfTargetDtm = GetHanaValue(reader, "AsOfTargetDtm");

                        astList.Add(ast);
                    }

                    DataTable dt = new DataTable();

                    dt.Columns.Add("[FacilityName]", typeof(string));
                    dt.Columns.Add("[TechnologyNodeName]", typeof(string));
                    dt.Columns.Add("[ProcessName]", typeof(string));
                    dt.Columns.Add("[FunctionalAreaName]", typeof(string));
                    dt.Columns.Add("[OrganizationAreaName]", typeof(string));
                    dt.Columns.Add("[OrganizationUnitName]", typeof(string));
                    dt.Columns.Add("[ParentCapitalEquipmentIdentifier]", typeof(string));
                    dt.Columns.Add("[CapitalEquipmentIdentifier]", typeof(string));
                    dt.Columns.Add("[SparePartNumber]", typeof(string));
                    dt.Columns.Add("[SparePartDescription]", typeof(string));
                    dt.Columns.Add("[SupplierIdentifier]", typeof(string));
                    dt.Columns.Add("[SupplierName]", typeof(string));
                    dt.Columns.Add("[SupplierPartNumber]", typeof(string));
                    dt.Columns.Add("[WorkWeekNumber]", typeof(string));
                    dt.Columns.Add("[UniqueEquipmentIdentifier]", typeof(string));
                    dt.Columns.Add("[ConsumedQuantity]", typeof(string));
                    dt.Columns.Add("[ConsumptionAmount]", typeof(string));
                    dt.Columns.Add("[CostToIntelAmount]", typeof(string));
                    dt.Columns.Add("[TransactionDateTime]", typeof(string));
                    dt.Columns.Add("[LastUpdatedDateTime]", typeof(string));
                    dt.Columns.Add("[AsOfSourceDtm]", typeof(string));
                    dt.Columns.Add("[AsOfTargetDtm]", typeof(string));

                    foreach (AllocatedSpareTransaction ast in astList)
                    //for (int i= 0;i<10;i++)
                    {
                        //OrphanedPO opo = orphanedPOList.ElementAt(i);
                        dt.Rows.Add(
                            ast.FacilityName,   
                            ast.TechnologyNodeName,
                            ast.ProcessName,
                            ast.FunctionalAreaName,
                            ast.OrganizationAreaName,
                            ast.OrganizationUnitName,
                            ast.ParentCapitalEquipmentIdentifier,
                            ast.CapitalEquipmentIdentifier,
                            ast.SparePartNumber,
                            ast.SparePartDescription,
                            ast.SupplierIdentifier,
                            ast.SupplierName,
                            ast.SupplierPartNumber,
                            ast.WorkWeekNumber,
                            ast.UniqueEquipmentIdentifier,
                            ast.ConsumedQuantity,
                            ast.ConsumptionAmount,
                            ast.CostToIntelAmount,
                            ast.TransactionDateTime,
                            ast.LastUpdatedDateTime,
                            ast.AsOfSourceDtm,
                            ast.AsOfTargetDtm
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
