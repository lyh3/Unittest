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
    internal class ParentCEIDtoCEIDMappingDataContext : IParentCEIDtoCEIDMappingDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ParentCEIDtoCEIDMappingDataContext()
        {

        }

        public void SyncParentCEIDtoCEIDMappingData()
        {
            log.Info("--> SyncParentCEIDtoCEIDMappingData");
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
                dbCommand.CommandText = Settings.ParentCeidMappingQuery;
                dbReader = dbCommand.ExecuteReader();
                ParentCEIDtoCEIDMappingList pceidList = new ParentCEIDtoCEIDMappingList();
                while (dbReader.Read())
                {
                    ParentCEIDtoCEIDMapping pceid = new ParentCEIDtoCEIDMapping();

                    pceid.TechnologyNodeNm = dbReader["TechnologyNodeNm"].ToStringSafely();
                    pceid.CapitalEquipmentIdentifier = dbReader["CapitalEquipmentIdentifier"].ToStringSafely();
                    pceid.ParentCapitalEquipmentIdentifier = dbReader["ParentCapitalEquipmentIdentifier"].ToStringSafely();
                    pceid.ActiveInd = dbReader["ActiveInd"].ToStringSafely();
                    pceid.AsOfSourceTs = dbReader["AsOfSourceTs"].ToStringSafely();
                    pceid.AsOfTargetTs = dbReader["AsOfTargetTs"].ToStringSafely();
                    pceidList.Add(pceid);
                }
                log.Info("Total number of records from ParentCEIDtoCEIDMappingData: " + pceidList.Count);
                UpdateParentCEIDtoCEIDMappingData(pceidList);
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
            log.Info("<-- SyncParentCEIDtoCEIDMappingData");
        }

        public void UpdateParentCEIDtoCEIDMappingData(ParentCEIDtoCEIDMappingList pceidList)
        {
            log.Info("--> UpdateParentCEIDtoCEIDMappingData");

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

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.ParentCEIDMappingSP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.ParentCEIDMappingTableType, dt);
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

            log.Info("<-- UpdateParentCEIDtoCEIDMappingData");

        }
    }
}
