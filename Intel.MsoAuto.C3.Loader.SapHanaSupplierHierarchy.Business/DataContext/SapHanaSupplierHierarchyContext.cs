using System.Data;
using Intel.MsoAuto.DataAccess;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.SapHanaSupplierHierarchy.Business.DataContext {
    public class SapHanaSupplierHierarchyContext : ISapHanaSupplierHierarchyContext {
        public bool SyncSapHanaSupplierHierarchyData()
        {
            bool result = false;
            try
            {
                ISqlDataAccess? dataAccess = null;
                DataTable dt = new DataTable();
                IConfigurationSection envConfig = Core.Settings.EnvConfig;
                string? connectionString = envConfig.GetRequiredAppSettingsValueValidation(Core.Constants.HANA_CONNECTION_STRING);
                string? hanaCommand = Core.Settings.Configuration.GetSection($"{Core.Constants.CONFIGURATIONS}:{Core.Constants.SUPPLIER_HEIRACHY_QUERY}").Value;

                using (HanaConnection conn = new HanaConnection(connectionString))
                {
                    Shared.Functions.LogInfo($"Connecting to {connectionString} ...");
                    conn.Open();
                    Shared.Functions.LogInfo(Core.Constants.CONNECTED);
                    HanaCommand cmd = new HanaCommand(hanaCommand, conn);
                    cmd.CommandType = CommandType.Text;
                    HanaDataReader reader = cmd.ExecuteReader();

                    dt.Columns.Add("SupplierHierarchyTypeNm", typeof(string));
                    dt.Columns.Add("GlobalUltimateSupplierId", typeof(string));
                    dt.Columns.Add("ParentSupplierId", typeof(string));
                    dt.Columns.Add("ChildSupplierId", typeof(string));
                    dt.Columns.Add("EffectiveStartDtm", typeof(string));
                    dt.Columns.Add("EffectiveEndDtm", typeof(string));
                    dt.Columns.Add("SupplierHierarchyRelationshipClassificationNm", typeof(string));
                    dt.Columns.Add("CreateAgentId", typeof(string));
                    dt.Columns.Add("CreateDtm", typeof(string));
                    dt.Columns.Add("ChangeAgentId", typeof(string));
                    dt.Columns.Add("ChangeDtm", typeof(string));
                    while (reader.Read())
                    {
                        DataRow r = dt.NewRow();
                        r["SupplierHierarchyTypeNm"] = reader["SupplierHierarchyTypeNm"].ToStringSafely();
                        r["GlobalUltimateSupplierId"] = reader["GlobalUltimateSupplierId"].ToStringSafely();
                        r["ParentSupplierId"] = reader["ParentSupplierId"].ToStringSafely();
                        r["ChildSupplierId"] = reader["ChildSupplierId"].ToStringSafely();
                        r["EffectiveStartDtm"] = reader["EffectiveStartDtm"].ToStringSafely();
                        r["EffectiveEndDtm"] = reader["EffectiveEndDtm"].ToStringSafely();
                        r["SupplierHierarchyRelationshipClassificationNm"] = reader["SupplierHierarchyRelationshipClassificationNm"].ToStringSafely();
                        r["CreateAgentId"] = reader["CreateAgentId"].ToStringSafely();
                        r["CreateDtm"] = reader["CreateDtm"].ToDateTimeSafely();
                        r["ChangeAgentId"] = reader["ChangeAgentId"].ToStringSafely();
                        r["ChangeDtm"] = reader["ChangeDtm"].ToStringSafely();
                        dt.Rows.Add(r);
                    }

                    dt.AcceptChanges();

                    string? c3CommonConnectionString = envConfig.GetRequiredAppSettingsValueValidation(Core.Constants.C3COMMON_CONNECTION_STRING);
                    dataAccess = new DataAccessFactory().CreateSqlDataAccess(c3CommonConnectionString, Core.Constants.UPDATE_SUPPLIER_HIERARCHY);
                    Shared.Functions.LogInfo($"Connect to C3Common [{c3CommonConnectionString}] success.");
                    dataAccess.AddInputParameter("@inputTable", dt);
                    dataAccess.SetTimeout(Core.Constants.SQL_EXEC_TIMEOUT);
                    dataAccess.Execute();
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}

