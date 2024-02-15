using Intel.MsoAuto.C3.Loader.CCSapHana.Business.Entity;
using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intel.MsoAuto.C3.Loader.CCSapHana.Business.Core;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess;
//using ExcelDataReader.Log;
using log4net;
using log4net.Config;
using System.Reflection;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.DataContext
{
    public class CopyExactIdentifierDataContext : ICopyExactIdentifierDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public CopyExactIdentifierDataContext()
        {

        }

        public string GetHanaValue(HanaDataReader reader, string colName)
        {
            return reader[colName] == null ? null : reader[colName].ToString();
        }

        public bool SyncCopyExactIdentifierData()
        {
            log.Info("--> SyncCopyExactIdentifierData");
            HanaConnection conn = null;
            HanaCommand cmd = null;
            HanaDataReader reader = null;
            try
            {
                string CeidDbConnString = Settings.ConnectionStringPreProd;
                string CeidQuery = Settings.CeidQuery;
                conn = new HanaConnection(CeidDbConnString);
                log.Info("Connecting...");
                conn.Open();
                log.Info("Connected...");
                cmd = new HanaCommand(CeidQuery, conn);
                cmd.CommandType = CommandType.Text;
                reader = cmd.ExecuteReader();
                CopyExactIdentifiers ceidList = new CopyExactIdentifiers();
                CopyExactIdentifier ceid = null;
                while (reader.Read())
                {
                    ceid = new CopyExactIdentifier();

                    ceid.CopyExactId = GetHanaValue(reader, "CopyExactlyId");
                    ceid.CapacityProcessCd = GetHanaValue(reader, "CapacityProcessCd");
                    ceid.ToolManufacturerId = GetHanaValue(reader, "ToolManufacturerId");
                    ceid.ToolManufacturerNm = GetHanaValue(reader, "ToolManufacturerNm");

                    ceidList.Add(ceid);
                }
                
                UpdateCopyExactIdentifierDataToDb(ceidList);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (reader!=null)
                    reader.Close();
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }           
            log.Info("<-- SyncCopyExactIdentifierData");
            return true;

        }

        public void UpdateCopyExactIdentifierDataToDb(CopyExactIdentifiers ceidList)
        {
            log.Info("--> UpdateCopyExactIdentifierDataToDb");
            DataTable dt = new DataTable();

            dt.Columns.Add("[CopyExactId]", typeof(string));
            dt.Columns.Add("[CapacityProcessCd]", typeof(string));
            dt.Columns.Add("[ToolManufacturerId]", typeof(string));
            dt.Columns.Add("[ToolManufacturerNm]", typeof(string));

            foreach (CopyExactIdentifier ceid in ceidList)
            //for (int i= 0;i<10;i++)
            {
                //OrphanedPO opo = orphanedPOList.ElementAt(i);
                dt.Rows.Add(
                    ceid.CopyExactId,
                    ceid.CapacityProcessCd,
                    ceid.ToolManufacturerId,
                    ceid.ToolManufacturerNm
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.CeidSP);
                dataAccess.AddInputParameter("@inputTable", dt);
                //dataAccess.AddTableValueParameter("@inputTable", Settings.DemandForecastTableType, dt);
                dataAccess.ExecuteReader();
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.Message);
                throw;
            }
            finally
            {
                if (dataAccess != null)
                {
                    dataAccess.Close();
                }
            }
            log.Info("<-- UpdateCopyExactIdentifierDataToDb");
        }
    }
}
