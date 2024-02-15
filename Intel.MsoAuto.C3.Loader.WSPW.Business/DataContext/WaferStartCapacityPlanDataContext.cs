using Intel.MsoAuto.C3.Loader.WSPW.Business.Entities;
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
using System.Data.Odbc;
using System.IO;
using System.Security.Policy;
using System.Diagnostics;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.DataAccess;
using Intel.MsoAuto.DataAccess.Teradata;
using Intel.MsoAuto.DataAccess.Oracle;
using System.Reflection.PortableExecutable;
using Intel.MsoAuto.C3.Loader.WSPW.Business.Core;
using System.Reflection;
using log4net;
using log4net.Config;
using Intel.MsoAuto.DataAccess.Sql;
using IdentityModel.OidcClient;
using System.Net.Mail;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.DataContext
{
    public class WaferStartCapacityPlanDataContext : IWaferStartCapacityPlanDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public WaferStartCapacityPlanDataContext()
        {            
        }

        public void SyncWaferStartCapacityPlanData()
        {
            log.Info("--> SyncWaferStartCapacityPlanData");
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
                dbCommand.CommandText = Settings.CapacityPlanCommand;
                dbReader = dbCommand.ExecuteReader();
                
                WaferStartCapacityPlans data = new WaferStartCapacityPlans();
                WaferStartCapacityPlan d = null;
                while (dbReader.Read())
                {
                    d = new WaferStartCapacityPlan();
                    d.AsOfSourceTs = dbReader["AsOfSourceTs"].ToStringSafely();
                    d.AsOfTargetTs = dbReader["AsOfTargetTs"].ToStringSafely();
                    d.SecurityLabelNm = dbReader["SecurityLabelNm"].ToStringSafely();
                    d.ProductRoadmapVersionId = dbReader["ProductRoadmapVersionId"].ToStringSafely();
                    d.ProductRoadmapVersionNm = dbReader["ProductRoadmapVersionNm"].ToStringSafely();
                    d.ProductRoadmapTypeCd = dbReader["ProductRoadmapTypeCd"].ToStringSafely();
                    d.ProductRoadmapSubTypeCd = dbReader["ProductRoadmapSubTypeCd"].ToStringSafely();
                    d.HorizonTimePeriodStartTxt = dbReader["HorizonTimePeriodStartTxt"].ToStringSafely();
                    d.HorizonTimePeriodEndTxt = dbReader["HorizonTimePeriodEndTxt"].ToStringSafely();
                    d.PlanCycleReleasedTimePeriodTxt = dbReader["PlanCycleReleasedTimePeriodTxt"].ToStringSafely();                 
                    d.FabricationProcessTechnologyCd = dbReader["FabricationProcessTechnologyCd"].ToStringSafely();
                    d.ManufacturingLocationCd = dbReader["ManufacturingLocationCd"].ToStringSafely();
                    d.SendingManufacturingLocationCd = dbReader["SendingManufacturingLocationCd"].ToStringSafely();
                    d.ReceivingManufacturingLocationCd = dbReader["ReceivingManufacturingLocationCd"].ToStringSafely();
                    d.SendingFabricationProcessTechnologyCd = dbReader["SendingFabricationProcessTechnologyCd"].ToStringSafely();
                    d.ReceivingFabricationProcessTechnologyCd = dbReader["ReceivingFabricationProcessTechnologyCd"].ToStringSafely();
                    d.FiscalYearNbr = dbReader["FiscalYearNbr"].ToStringSafely();
                    d.FiscalQuarterNbr = dbReader["FiscalQuarterNbr"].ToStringSafely();
                    d.FiscalMonthNbr = dbReader["FiscalMonthNbr"].ToStringSafely();
                    d.PlanCycleStartDt = dbReader["PlanCycleStartDt"].ToStringSafely();
                    d.ChangeDtm = dbReader["ChangeDtm"].ToStringSafely();
                    d.C4EquippedCapacityQty = dbReader["C4EquippedCapacityQty"].ToStringSafely();
                    d.CommitTradeCapacityQty = dbReader["CommitTradeCapacityQty"].ToStringSafely();
                    d.EquippedTradeCapacityQty = dbReader["EquippedTradeCapacityQty"].ToStringSafely();
                    d.EquippedCapacityQty = dbReader["EquippedCapacityQty"].ToStringSafely();
                    d.CommitCapacityQty = dbReader["CommitCapacityQty"].ToStringSafely();
                    d.WholeCommitCapacityQty = dbReader["WholeCommitCapacityQty"].ToStringSafely();
                    d.CommitCapacityAdjustmentQty = dbReader["CommitCapacityAdjustmentQty"].ToStringSafely();
                    d.ProductionTradeRatioQty = dbReader["ProductionTradeRatioQty"].ToStringSafely();
                    d.FabEngineeringTradeRatioQty = dbReader["FabEngineeringTradeRatioQty"].ToStringSafely();
                    d.DivisionalEngineeringTradeRatioQty = dbReader["DivisionalEngineeringTradeRatioQty"].ToStringSafely();
                    d.TechnologyDevelopmentEngineeringTradeRatioQty = dbReader["TechnologyDevelopmentEngineeringTradeRatioQty"].ToStringSafely();
                    d.FactoryPlannedEventImpactQty = dbReader["FactoryPlannedEventImpactQty"].ToStringSafely();
                    d.IP_RoadmapReleasedYear = dbReader["IP_RoadmapReleasedYear"].ToStringSafely();
                    d.C4SIBEquippedCapacityQty = dbReader["C4SIBEquippedCapacityQty"]==null?"":dbReader["C4SIBEquippedCapacityQty"].ToStringSafely();
                    d.C4PSBEquippedCapacityQty = dbReader["C4PSBEquippedCapacityQty"] == null ? "" : dbReader["C4PSBEquippedCapacityQty"].ToStringSafely();
                    data.Add(d);
                }
                log.Info("Total number of records from WaferStartCapacityPlan: " + data.Count);
                UpdateWaferStartCapacityPlanDataToDb(data);
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
            log.Info("<-- SyncWaferStartCapacityPlanData");
        }

        private void UpdateWaferStartCapacityPlanDataToDb(WaferStartCapacityPlans data)
        {
            log.Info("--> UpdateWaferStartCapacityPlanDataToDb");
            /*
            1.EquippedCapacityQty = wafer starts per week
            2.FabricationProcessTechnologyCd = process
            3.ManufacturingLocationCd = facility
            4.FiscalYearNbr = year
            5.FiscalQuarterNbr = quarter
            */
            DataTable dt = new DataTable();
      
            dt.Columns.Add("FabricationProcessTechnologyCd", typeof(string));
            dt.Columns.Add("ManufacturingLocationCd", typeof(string));
            dt.Columns.Add("ReceivingManufacturingLocationCd", typeof(string));
            dt.Columns.Add("FiscalYearNbr", typeof(string));
            dt.Columns.Add("FiscalQuarterNbr", typeof(string));
            dt.Columns.Add("FiscalMonthNbr", typeof(string));
            dt.Columns.Add("EquippedCapacityQty", typeof(string));  
            dt.Columns.Add("C4EquippedCapacityQty", typeof(string));
            dt.Columns.Add("C4SIBEquippedCapacityQty", typeof(string));
            dt.Columns.Add("C4PSBEquippedCapacityQty", typeof(string));

            foreach (WaferStartCapacityPlan d in data)
            {
                dt.Rows.Add(
                    d.FabricationProcessTechnologyCd,
                    d.ManufacturingLocationCd,                  
                    d.ReceivingManufacturingLocationCd,         
                    d.FiscalYearNbr,
                    d.FiscalQuarterNbr,
                    d.FiscalMonthNbr,                  
                    d.EquippedCapacityQty,                
                    d.C4EquippedCapacityQty,
                    d.C4SIBEquippedCapacityQty,
                    d.C4PSBEquippedCapacityQty
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.CapacityPlanSP);
                dataAccess.AddTableValueParameter("@inputTable", Settings.CapacityPlanTableType, dt);
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

            log.Info("<-- UpdateWaferStartCapacityPlanDataToDb");
        }
    }
}
