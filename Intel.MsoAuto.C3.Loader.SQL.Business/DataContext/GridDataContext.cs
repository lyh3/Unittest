using Intel.MsoAuto.C3.Loader.SQL.Business.Entities;
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
using Intel.MsoAuto.C3.Loader.SQL.Business.Core;
using System.Reflection;
using log4net;
using log4net.Config;
using Intel.MsoAuto.DataAccess.Sql;
using IdentityModel.OidcClient;
using System.Net.Mail;

namespace Intel.MsoAuto.C3.Loader.SQL.Business.DataContext
{
    public class GridDataContext : IGridDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public GridDataContext()
        {            
        }

        public void SyncGridSupplierContactData()
        {
            log.Info("--> SyncGridSupplierContactData");

            ISqlDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.GridConnectionString, Settings.SupplierContactQuery, CommandType.Text);
                dataReader = dataAccess.ExecuteReader();

                GridSupplierContacts data = new GridSupplierContacts();
                GridSupplierContact d = null;
                while (dataReader.Read())
                {
                    d = new GridSupplierContact();
                    d.SupplierId = dataReader["SupplierId"].ToStringSafely();
                    d.SupplierName = dataReader["SupplierName"].ToStringSafely();
                    d.Name = dataReader["Name"].ToStringSafely();
                    d.Role = dataReader["role"].ToStringSafely();
                    d.Email = dataReader["Email"].ToStringSafely();
                    d.PhoneNumber = dataReader["PhoneNumber"].ToStringSafely();
                    d.Title = dataReader["Title"].ToStringSafely();
                    d.SupplierStatus = dataReader["SupplierStatus"].ToStringSafely();
                    d.InActive = dataReader["InActive"].ToStringSafely();
                    d.IsDeleted = dataReader["IsDeleted"].ToStringSafely();
                    d.LastUpdated = dataReader["lastupdated"].ToStringSafely();
                    data.Add(d);
                }
                log.Info("Total number of records from v_SupplierContact: " + data.Count);
                UpdateSupplierContactDataToDb(data);
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.InnerException);
                throw;
            }
            finally
            {
                if (dataReader.IsNotNull())
                    dataReader.Close();
                if (dataAccess.IsNotNull())
                    dataAccess.Close();
            }
            log.Info("<-- SyncGridSupplierContactData");
        }

        public void SyncGridSupplierIntelContactData()
        {
            log.Info("--> SyncGridSupplierIntelContactData");

            ISqlDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.GridConnectionString, Settings.SupplierIntelContactQuery, CommandType.Text);
                dataReader = dataAccess.ExecuteReader();

                GridSupplierIntelContacts data = new GridSupplierIntelContacts();
                GridSupplierIntelContact d = null;
                while (dataReader.Read())
                {
                    d = new GridSupplierIntelContact();
                    d.SupplierId = dataReader["SupplierId"].ToStringSafely();
                    d.SupplierName = dataReader["SupplierName"].ToStringSafely();
                    d.Wwid = dataReader["Wwid"].ToStringSafely();
                    d.Name = dataReader["Name"].ToStringSafely();
                    d.Idsid = dataReader["Idsid"].ToStringSafely();
                    d.Status = dataReader["status"].ToStringSafely();
                    d.Role = dataReader["Role"].ToStringSafely();
                    d.IsDeleted = dataReader["Isdeleted"].ToStringSafely();
                    d.SupplierStatus = dataReader["SupplierStatus"].ToStringSafely();
                    d.InActive = dataReader["InActive"].ToStringSafely();
                    data.Add(d);
                }
                log.Info("Total number of records from v_SupplierIntelContact: " + data.Count);
                UpdateSupplierIntelContactDataToDb(data);
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.InnerException);
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataReader.IsNotNull())
                    dataReader.Close();
                if (dataAccess.IsNotNull())
                    dataAccess.Close();
            }
            log.Info("<-- SyncGridSupplierIntelContactData");
        }

        private void UpdateSupplierIntelContactDataToDb(GridSupplierIntelContacts data)
        {
            log.Info("--> UpdateSupplierIntelContactDataToDb");

            DataTable dt = new DataTable();
            dt.Columns.Add("SupplierId", typeof(string));
            dt.Columns.Add("SupplierName", typeof(string));
            dt.Columns.Add("Wwid", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Idsid", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Role", typeof(string));
            dt.Columns.Add("IsDeleted", typeof(string));
            dt.Columns.Add("SupplierStatus", typeof(string));
            dt.Columns.Add("InActive", typeof(string));

            foreach (GridSupplierIntelContact d in data)
            {
                dt.Rows.Add(
                    d.SupplierId,
                    d.SupplierName,
                    d.Wwid,
                    d.Name,
                    d.Idsid,
                    d.Status,
                    d.Role,
                    d.IsDeleted,
                    d.SupplierStatus,
                    d.InActive
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.SupplierIntelContactSP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.SupplierIntelContactTableType, dt);
                dataAccess.ExecuteReader();
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }

            log.Info("<-- UpdateSupplierIntelContactDataToDb");
        }

        private void UpdateSupplierContactDataToDb(GridSupplierContacts data)
        {
            log.Info("--> UpdateSupplierContactDataToDb");
           
            DataTable dt = new DataTable();
            dt.Columns.Add("SupplierId", typeof(string));
            dt.Columns.Add("SupplierName", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Role", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("PhoneNumber", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("SupplierStatus", typeof(string));
            dt.Columns.Add("InActive", typeof(string));
            dt.Columns.Add("IsDeleted", typeof(string));
            dt.Columns.Add("LastUpdated", typeof(string));

            foreach (GridSupplierContact d in data)
            {
                dt.Rows.Add(
                    d.SupplierId,
                    d.SupplierName,
                    d.Name,
                    d.Role,
                    d.Email,
                    d.PhoneNumber,
                    d.Title,
                    d.SupplierStatus,
                    d.InActive,
                    d.IsDeleted,
                    d.LastUpdated
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.SupplierContactSP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.SupplierContactTableType, dt);
                dataAccess.ExecuteReader();                
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }

            log.Info("<-- UpdateSupplierContactDataToDb");
        }
    }
}
