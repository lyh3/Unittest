using Intel.MsoAuto.C3.Loader.WSPW.Business.Entities;
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
using System.Data.Odbc;
using Intel.MsoAuto.C3.Loader.WSPW.Business.Core;
using System.Reflection;
using log4net;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.DataContext
{
    internal class AllocatedGasChemTransactionDataContext : IAllocatedGasChemTransactionDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public AllocatedGasChemTransactionDataContext()
        {
        }

        public void SyncAllocatedGasChemTransactionData()
        {
            log.Info("--> SyncAllocatedGasChemTransactionData");
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
                dbCommand.CommandText = Settings.AllocatedGasChemQuery;
                dbReader = dbCommand.ExecuteReader();
                AllocatedGasChemTransactionList agctList = new AllocatedGasChemTransactionList();
                while (dbReader.Read())
                {
                    AllocatedGasChemTransaction agct = new AllocatedGasChemTransaction();

                    agct.FacilityNm = dbReader["FacilityNm"].ToStringSafely();
                    agct.TechnologyNodeNm = dbReader["TechnologyNodeNm"].ToStringSafely();
                    agct.ProcessNm = dbReader["ProcessNm"].ToStringSafely();
                    agct.FunctionalAreaNm = dbReader["FunctionalAreaNm"].ToStringSafely();
                    agct.OrganizationAreaNm = dbReader["OrganizationAreaNm"].ToStringSafely();
                    agct.OrganizationUnitNm = dbReader["OrganizationUnitNm"].ToStringSafely();
                    agct.ParentCapitalEquipmentId = dbReader["ParentCapitalEquipmentId"].ToStringSafely();
                    agct.CapitalEquipmentId = dbReader["CapitalEquipmentId"].ToStringSafely();
                    agct.GasChemGroupNm = dbReader["GasChemGroupNm"].ToStringSafely();
                    agct.GasChemNm = dbReader["GasChemNm"].ToStringSafely();
                    agct.GasChemDsc = dbReader["GasChemDsc"].ToStringSafely();
                    agct.SupplierNm = dbReader["SupplierNm"].ToStringSafely();
                    agct.WorkWeekNbr = dbReader["WorkWeekNbr"].ToStringSafely();
                    agct.UniqueEquipmentId = dbReader["UniqueEquipmentId"].ToStringSafely();
                    agct.AllocatedGasChemQty = dbReader["AllocatedGasChemQty"].ToStringSafely();
                    agct.SupplierId = dbReader["SupplierId"].ToStringSafely();
                    agct.TransactionDtm = dbReader["TransactionDtm"].ToStringSafely();
                    agct.LastUpdatedDtm = dbReader["LastUpdatedDtm"].ToStringSafely();
                    agct.AsOfSourceDtm = dbReader["AsOfSourceDtm"].ToStringSafely();
                    agct.AsOfTargetDtm = dbReader["AsOfTargetDtm"].ToStringSafely();
                    agct.UnitCostAmt = dbReader["UnitCostAmt"].ToStringSafely();
                    agct.ConsumptionValueAmt = dbReader["ConsumptionValueAmt"].ToStringSafely();
                    agct.FixedCostConsumedQty = dbReader["FixedCostConsumedQty"].ToStringSafely();
                    agct.ConsumptionRatioPct = dbReader["ConsumptionRatioPct"].ToStringSafely();
                    agct.UnitOfMeasureCd = dbReader["UnitOfMeasureCd"].ToStringSafely();

                    agctList.Add(agct);
                }
                log.Info("Total number of records from AllocatedGasChemTransactionData: " + agctList.Count);
                UpdateAllocatedGasChemTransactionData(agctList);
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
            log.Info("<-- SyncAllocatedGasChemTransactionData");
        }

        public void UpdateAllocatedGasChemTransactionData(AllocatedGasChemTransactionList agctList)
        {
            log.Info("--> UpdateAllocatedGasChemTransactionData");

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
            dt.Columns.Add("[UnitCostAmt]", typeof(string));
            dt.Columns.Add("[ConsumptionValueAmt]", typeof(string));
            dt.Columns.Add("[FixedCostConsumedQty]", typeof(string));
            dt.Columns.Add("[ConsumptionRatioPct]", typeof(string));
            dt.Columns.Add("[UnitOfMeasureCd]", typeof(string));

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
                    agct.AsOfTargetDtm,
                    agct.UnitCostAmt,
                    agct.ConsumptionValueAmt,
                    agct.FixedCostConsumedQty,
                    agct.ConsumptionRatioPct,
                    agct.UnitOfMeasureCd
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.GasChemSP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.GasChemTransactionTableType, dt);
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

            log.Info("<-- UpdateAllocatedGasChemTransactionData");

        }
    }
}
