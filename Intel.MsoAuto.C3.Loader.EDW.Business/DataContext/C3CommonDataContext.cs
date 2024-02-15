using Intel.MsoAuto.C3.Loader.EDW.Business.Core;
using Intel.MsoAuto.DataAccess;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess.Teradata;
using Intel.MsoAuto.Shared;
using Intel.MsoAuto.Shared.Extensions;
using System.Data;
using System.Security.Principal;

namespace Intel.MsoAuto.C3.Loader.EDW.Business.DataContext {
    public class C3CommonDataContext : IC3CommonDataContext {
        public void SyncEdwData()
        {
            DataTable? dt = null;
            dt = QueryEdwData();
            if (dt != null)
                UpdateStageData(dt);
        }
        private static DataTable QueryEdwData()
        {
            Functions.LogInfo("--> QueryEdwData.");
            ITeradataDataAccess? teraDataAccess = null;
            IDataReader? dataReader = null;
            DataTable dt = new DataTable();
            try
            {
                teraDataAccess = new DataAccessFactory().CreateTeraDataAccess(Settings.TeraDataConfig.GetSection(Core.Constants.TERA_DATA_CONNECTION_STRING).Value,
                                                                              Settings.TeraDataConfig.GetSection(Core.Constants.CEID_MAPPING_QUERY).Value,
                                                                              CommandType.Text);

                dataReader = teraDataAccess.ExecuteReader();

                dt.Columns.Add("CEID", typeof(string));
                dt.Columns.Add("EquipmentType", typeof(string));
                while (dataReader.Read())
                {
                    DataRow r = dt.NewRow();
                    string? ceid = dataReader["CEID"].ToStringSafely();
                    if (string.IsNullOrEmpty(ceid) || ceid == Core.Constants.NO_CEID)
                        continue;
                    r["CEID"] = ceid;
                    r["EquipmentType"] = dataReader["EquipmentType"].ToStringSafely();
                    dt.Rows.Add(r);
                }
                dt.AcceptChanges();
                Functions.LogInfo("<-- success, QueryEdwData.");
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
                if (teraDataAccess != null)
                    teraDataAccess.Close();
            }
            return dt;
        }
        private static void UpdateStageData(DataTable dt)
        {
            Functions.LogInfo("--> UpdateStageData.");
            ISqlDataAccess? dataAccess = null;
            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.EnvConfig.GetSection(Core.Constants.C3COMMON_CONNECTION_STRING).Value, Core.Constants.UPDATE_EDW_DATA);
                dataAccess.AddInputParameter("@EdwData", dt);
                dataAccess.SetTimeout(Core.Constants.SQL_EXEC_TIMEOUT);
                dataAccess.Execute();
                Functions.LogInfo("<-- success, UpdateStageData.");
           }
            finally
            {
                if (dataAccess != null)
                    dataAccess.Close();
            }
        }
    }
}