using Intel.MsoAuto.C3.Loader.WSPW.Business.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Reflection;
using Intel.MsoAuto.C3.Loader.WSPW.Business.Core;
using log4net;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess;
using System.Data.Odbc;
using System.Reflection.PortableExecutable;


namespace Intel.MsoAuto.C3.Loader.WSPW.Business.DataContext
{
    internal class ParentCeidPercentWspwDataContext : IParentCeidPercentWspwDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ParentCeidPercentWspwDataContext()
        {

        }

        public void SyncParentCeidPercentWspwData()
        {
            log.Info("--> SyncParentCeidPercentWspwData");
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
                dbCommand.CommandText = Settings.ParentCeidPercentWspwQuery;
                dbReader = dbCommand.ExecuteReader();
                ParentCeidPercentWspwList pcpwList = new ParentCeidPercentWspwList();
                while (dbReader.Read())
                {
                    ParentCeidPercentWspw pcpw = new ParentCeidPercentWspw();
                    pcpw.AsOfSourceDtm = dbReader["AsOfSourceDtm"].ToStringSafely();
                    pcpw.AsOfTargetDtm = dbReader["AsOfTargetDtm"].ToStringSafely();
                    pcpw.CommodityNm = dbReader["CommodityNm"].ToStringSafely();
                    pcpw.FacilityNm = dbReader["FacilityNm"].ToStringSafely();
                    pcpw.FiveKToolCnt = dbReader["FiveKToolCnt"].ToStringSafely();
                    pcpw.FiveKWaferStartPerWeekPct = dbReader["FiveKWaferStartPerWeekPct"].ToStringSafely();
                    pcpw.LastUpdatedDtm = dbReader["LastUpdatedDtm"].ToStringSafely();
                    pcpw.OrganizationAreaNm = dbReader["OrganizationAreaNm"].ToStringSafely();
                    pcpw.OrganizationUnitNm = dbReader["OrganizationUnitNm"].ToStringSafely();
                    pcpw.ParentCapitalEquipmentId = dbReader["ParentCapitalEquipmentId"].ToStringSafely();
                    pcpw.ScalingWaferStartEquivalentRte = dbReader["ScalingWaferStartEquivalentRte"].ToStringSafely();
                    pcpw.TechnologyNodeNm = dbReader["TechnologyNodeNm"].ToStringSafely();
                    pcpw.ToolCnt = dbReader["ToolCnt"].ToStringSafely();
                    pcpw.VariablePct = dbReader["VariablePct"].ToStringSafely();
                    pcpw.WorkWeekNbr = dbReader["WorkWeekNbr"].ToStringSafely();

                    pcpwList.Add(pcpw);
                }
                log.Info("Total number of records from ParentCeidPercentWspw data: " + pcpwList.Count);
                UpdateParentCeidPercentWspwData(pcpwList);
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
            log.Info("<-- SyncParentCeidPercentWspwData");
        }

        public void UpdateParentCeidPercentWspwData(ParentCeidPercentWspwList pcpwList)
        {
            log.Info("--> UpdateParentCeidPercentWspwData");

            DataTable dt = new DataTable();

            dt.Columns.Add("[AsOfSourceDtm]", typeof(string));
            dt.Columns.Add("[AsOfTargetDtm]", typeof(string));
            dt.Columns.Add("[CommodityNm]", typeof(string));
            dt.Columns.Add("[FacilityNm]", typeof(string));
            dt.Columns.Add("[FiveKToolCnt]", typeof(string));
            dt.Columns.Add("[FiveKWaferStartPerWeekPct]", typeof(string));
            dt.Columns.Add("[LastUpdatedDtm]", typeof(string));
            dt.Columns.Add("[OrganizationAreaNm]", typeof(string));
            dt.Columns.Add("[OrganizationUnitNm]", typeof(string));
            dt.Columns.Add("[ParentCapitalEquipmentId]", typeof(string));
            dt.Columns.Add("[ScalingWaferStartEquivalentRte]", typeof(string));
            dt.Columns.Add("[TechnologyNodeNm]", typeof(string));
            dt.Columns.Add("[ToolCnt]", typeof(string));
            dt.Columns.Add("[VariablePct]", typeof(string));
            dt.Columns.Add("[WorkWeekNbr]", typeof(string));

            foreach (ParentCeidPercentWspw pcpw in pcpwList)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    pcpw.AsOfSourceDtm,
                    pcpw.AsOfTargetDtm,
                    pcpw.CommodityNm,
                    pcpw.FacilityNm,
                    pcpw.FiveKToolCnt,
                    pcpw.FiveKWaferStartPerWeekPct,
                    pcpw.LastUpdatedDtm,
                    pcpw.OrganizationAreaNm,
                    pcpw.OrganizationUnitNm,
                    pcpw.ParentCapitalEquipmentId,
                    pcpw.ScalingWaferStartEquivalentRte,
                    pcpw.TechnologyNodeNm,
                    pcpw.ToolCnt,
                    pcpw.VariablePct,
                    pcpw.WorkWeekNbr
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.ParentCeidPercentWspwSP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.ParentCeidPercentWspwTableType, dt);
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

            log.Info("<-- UpdateParentCeidPercentWspwData");

        }
    }
}
