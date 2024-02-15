using Intel.MsoAuto.C3.Loader.WSPW.Business.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Reflection.PortableExecutable;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess;
using System.Data.Odbc;
using System.Reflection;
using Intel.MsoAuto.C3.Loader.WSPW.Business.Core;
using System.Reflection;
using log4net;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.DataContext
{
    internal class AllocatedSpareTransactionDataContext : IAllocatedSpareTransactionDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public AllocatedSpareTransactionDataContext()
        {

        }

        public void SyncAllocatedSpareTransactionData()
        {
            log.Info("--> SyncAllocatedSpareTransactionData");
            OdbcDataReader dbReader = null;
            OdbcConnection dbConnection = null;
            OdbcCommand dbCommand = null;

            try
            {
                string odbcConn = Settings.WspwConnectionString;
                dbConnection = new OdbcConnection(odbcConn);
                log.Info("Connecting...");
                dbConnection.Open();
                log.Info("Connected...");
                dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = Settings.AllocatedSpareQuery;
                dbReader = dbCommand.ExecuteReader();
                AllocatedSpareTransactionList astList = new AllocatedSpareTransactionList();
                while (dbReader.Read())
                {
                    AllocatedSpareTransaction ast = new AllocatedSpareTransaction();
                    ast.FacilityName = dbReader["FacilityName"].ToStringSafely();
                    ast.TechnologyNodeName = dbReader["TechnologyNodeName"].ToStringSafely();
                    ast.ProcessName = dbReader["ProcessName"].ToStringSafely();
                    ast.FunctionalAreaName = dbReader["FunctionalAreaName"].ToStringSafely();
                    ast.OrganizationAreaName = dbReader["OrganizationAreaName"].ToStringSafely();
                    ast.OrganizationUnitName = dbReader["OrganizationUnitName"].ToStringSafely();
                    ast.ParentCapitalEquipmentIdentifier = dbReader["ParentCapitalEquipmentIdentifier"].ToStringSafely();
                    ast.CapitalEquipmentIdentifier = dbReader["CapitalEquipmentIdentifier"].ToStringSafely();
                    ast.SparePartNumber = dbReader["SparePartNumber"].ToStringSafely();
                    ast.SparePartDescription = dbReader["SparePartDescription"].ToStringSafely();
                    ast.SupplierIdentifier = dbReader["SupplierIdentifier"].ToStringSafely();
                    ast.SupplierName = dbReader["SupplierName"].ToStringSafely();
                    ast.SupplierPartNumber = dbReader["SupplierPartNumber"].ToStringSafely();
                    ast.WorkWeekNumber = dbReader["WorkWeekNumber"].ToStringSafely();
                    ast.UniqueEquipmentIdentifier = dbReader["UniqueEquipmentIdentifier"].ToStringSafely();
                    ast.ConsumedQuantity = dbReader["ConsumedQuantity"].ToStringSafely();
                    ast.ConsumptionAmount = dbReader["ConsumptionAmount"].ToStringSafely();
                    ast.CostToIntelAmount = dbReader["CostToIntelAmount"].ToStringSafely();
                    ast.TransactionDateTime = dbReader["TransactionDateTime"].ToStringSafely();
                    ast.LastUpdatedDateTime = dbReader["LastUpdatedDateTime"].ToStringSafely();
                    ast.AsOfSourceDtm = dbReader["AsOfSourceDtm"].ToStringSafely();
                    ast.AsOfTargetDtm = dbReader["AsOfTargetDtm"].ToStringSafely();

                    astList.Add(ast);
                }
                log.Info("Total number of records from AllocatedSpareTransactionData: " + astList.Count);
                UpdateAllocatedSparesTransactionData(astList);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (dbReader.IsNotNull())
                    dbReader.Close();
                if (dbCommand.IsNotNull())
                    dbCommand.Dispose();
                if (dbConnection.IsNotNull())
                    dbConnection.Close();
            }
            log.Info("<-- SyncAllocatedSpareTransactionData");
        }

        public void UpdateAllocatedSparesTransactionData(AllocatedSpareTransactionList astList)
        {
            log.Info("--> UpdateAllocatedSparesTransactionData");

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

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.SpareTransactionSP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.SpareTransactionTableType, dt);
                dataAccess.ExecuteReader();
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.Message);
                throw;
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }

            log.Info("<-- UpdateAllocatedSparesTransactionData");

        }
    }
}
