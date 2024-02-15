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
using Intel.MsoAuto.Shared.Extensions;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using Intel.MsoAuto.C3.Loader.WSPW.Business.Core;
using System.Reflection;
using log4net;
using log4net.Config;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.DataContext
{
    public class WaferStartDemandForecastDataContext: IWaferStartDemandForecastDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WaferStartDemandForecastDataContext()
        {
        }

        public void SyncWaferStartDemandForecastData()
        {
            log.Info("--> SyncWaferStartDemandForecastData");
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
                dbCommand.CommandText = Settings.DemandForecastCommand;
                //DbCommand.CommandText = "SELECT * FROM \"supply\".\"WaferStartLongRangeDemandForecast\" where \"IP_RoadmapReleaseYear\"='2021' and \"ManufacturingLocationCd\"='F24' and \"ItemId\"='2000-068-124'";
                //DbCommand.CommandText = "SELECT * FROM \"supply\".\"WaferStartLongRangeDemandForecast\" where \"IP_RoadmapReleaseYear\"='2023'";
                //DbCommand.CommandText = "SELECT distinct \"FiscalYearNbr\", \"FiscalQuarterNbr\" FROM \"supply\".\"WaferStartLongRangeDemandForecast\" where \"IP_RoadmapReleaseYear\"='2021' order by \"FiscalYearNbr\", \"FiscalQuarterNbr\"";
                //DbCommand.CommandText = "SELECT * FROM \"supply\".\"WaferStartLongRangeCapacityPlanning\" where \"IP_RoadmapReleasedYear\"='2021' and \"ManufacturingLocationCd\"='F24' and \"FiscalMonthNbr\"='07' and \"ProductRoadmapSubTypeCd\"='POR'";
                //DbCommand.CommandText = "SELECT distinct \"FiscalYearNbr\", \"FiscalQuarterNbr\", \"FiscalMonthNbr\" FROM \"supply\".\"WaferStartLongRangeCapacityPlanning\" where \"IP_RoadmapReleasedYear\"='2021'  order by \"FiscalYearNbr\", \"FiscalQuarterNbr\", \"FiscalMonthNbr\"";
                dbReader = dbCommand.ExecuteReader();
                int fCount = dbReader.FieldCount;

                WaferStartDemandForecasts data = new WaferStartDemandForecasts();
                WaferStartDemandForecast d = null;
                while (dbReader.Read())
                {
                    d = new WaferStartDemandForecast();
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
                    d.PlanCycleStartDt = dbReader["PlanCycleStartDt"].ToStringSafely();
                    d.ChangeDtm = dbReader["ChangeDtm"].ToStringSafely();
                    d.FabricationProcessTechnologyCd = dbReader["FabricationProcessTechnologyCd"].ToStringSafely();
                    d.ManufacturingLocationCd = dbReader["ManufacturingLocationCd"].ToStringSafely();
                    d.ItemId = dbReader["ItemId"].ToStringSafely();
                    d.ItemCharacteristicDerivedValueTxt = dbReader["ItemCharacteristicDerivedValueTxt"].ToStringSafely();
                    d.DotProcessValueTxt = dbReader["DotProcessValueTxt"].ToStringSafely();
                    d.ItemCodeNm = dbReader["ItemCodeNm"].ToStringSafely();
                    d.FiscalYearNbr = dbReader["FiscalYearNbr"].ToStringSafely();
                    d.FiscalQuarterNbr = dbReader["FiscalQuarterNbr"].ToStringSafely();
                    d.ProductStartQty = dbReader["ProductStartQty"].ToStringSafely();
                    d.DivisionalEngineeringStartUnallocatedQty = dbReader["DivisionalEngineeringStartUnallocatedQty"].ToStringSafely();
                    d.FabEngineeringStartUnallocatedQty = dbReader["FabEngineeringStartUnallocatedQty"].ToStringSafely();
                    d.TechnologyDevelopmentEngineeringStartUnallocatedQty = dbReader["TechnologyDevelopmentEngineeringStartUnallocatedQty"].ToStringSafely();
                    d.IP_RoadmapReleaseYear = dbReader["IP_RoadmapReleaseYear"].ToStringSafely();
                    data.Add(d);
                }
                UpdateWaferStartDemandForecastDataToDb(data);                
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
            log.Info("<-- SyncWaferStartDemandForecastData");
        }

        private void UpdateWaferStartDemandForecastDataToDb(WaferStartDemandForecasts data)
        {
            log.Info("--> UpdateWaferStartDemandForecastDataToDb");
            DataTable dt = new DataTable();
            dt.Columns.Add("AsOfSourceTs", typeof(string));
            dt.Columns.Add("AsOfTargetTs", typeof(string));
            dt.Columns.Add("SecurityLabelNm", typeof(string));
            dt.Columns.Add("ProductRoadmapVersionId", typeof(string));
            dt.Columns.Add("ProductRoadmapVersionNm", typeof(string));
            dt.Columns.Add("ProductRoadmapTypeCd", typeof(string));
            dt.Columns.Add("ProductRoadmapSubTypeCd", typeof(string));
            dt.Columns.Add("HorizonTimePeriodStartTxt", typeof(string));
            dt.Columns.Add("HorizonTimePeriodEndTxt", typeof(string));
            dt.Columns.Add("PlanCycleReleasedTimePeriodTxt", typeof(string));
            dt.Columns.Add("PlanCycleStartDt", typeof(string));
            dt.Columns.Add("ChangeDtm", typeof(string));
            dt.Columns.Add("FabricationProcessTechnologyCd", typeof(string));
            dt.Columns.Add("ManufacturingLocationCd", typeof(string));
            dt.Columns.Add("ItemId", typeof(string));
            dt.Columns.Add("ItemCharacteristicDerivedValueTxt", typeof(string));
            dt.Columns.Add("DotProcessValueTxt", typeof(string));
            dt.Columns.Add("ItemCodeNm", typeof(string));
            dt.Columns.Add("FiscalYearNbr", typeof(string));
            dt.Columns.Add("FiscalQuarterNbr", typeof(string));
            dt.Columns.Add("DivisionalEngineeringStartUnallocatedQty", typeof(string));
            dt.Columns.Add("FabEngineeringStartUnallocatedQty", typeof(string));
            dt.Columns.Add("TechnologyDevelopmentEngineeringStartUnallocatedQty", typeof(string));
            dt.Columns.Add("IP_RoadmapReleaseYear", typeof(string));

            foreach (WaferStartDemandForecast d in data)
            {
                dt.Rows.Add(
                    d.AsOfSourceTs,
                    d.AsOfTargetTs,
                    d.SecurityLabelNm,
                    d.ProductRoadmapVersionId,
                    d.ProductRoadmapVersionNm,
                    d.ProductRoadmapTypeCd,
                    d.ProductRoadmapSubTypeCd,
                    d.HorizonTimePeriodStartTxt,
                    d.HorizonTimePeriodEndTxt,
                    d.PlanCycleReleasedTimePeriodTxt,
                    d.PlanCycleStartDt,
                    d.ChangeDtm,
                    d.FabricationProcessTechnologyCd,
                    d.ManufacturingLocationCd,
                    d.ItemId,
                    d.ItemCharacteristicDerivedValueTxt,
                    d.DotProcessValueTxt,
                    d.ItemCodeNm,
                    d.FiscalYearNbr,
                    d.FiscalQuarterNbr,
                    d.ProductStartQty,
                    d.DivisionalEngineeringStartUnallocatedQty,
                    d.FabEngineeringStartUnallocatedQty,
                    d.TechnologyDevelopmentEngineeringStartUnallocatedQty,
                    d.IP_RoadmapReleaseYear
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.DemandForecastSP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.DemandForecastTableType, dt);
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
            log.Info("<-- UpdateWaferStartDemandForecastDataToDb");

        }
    }
}
