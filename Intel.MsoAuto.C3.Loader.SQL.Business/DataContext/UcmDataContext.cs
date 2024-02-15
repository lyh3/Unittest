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
using Intel.MsoAuto.Shared.Extensions;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using Intel.MsoAuto.C3.Loader.SQL.Business.Core;
using System.Reflection;
using log4net;
using log4net.Config;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess;

namespace Intel.MsoAuto.C3.Loader.SQL.Business.DataContext
{
    public class UcmDataContext : IUcmDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UcmDataContext()
        {
        }

        public void SyncUcmData()
        {
            log.Info("--> SyncUcmData");
            ISqlDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                // Create data access object with UcmConnectionString and UcmQuery from settings
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.UcmConnectionString, Settings.UcmQuery, CommandType.Text);

                // Execute query and retrieve reader
                dataReader = dataAccess.ExecuteReader();

                // Create new UcmRecords object to store retrieved data
                UcmRecords data = new UcmRecords();

                // Create new UcmRecord object to store each row of data retrieved
                UcmRecord d = null;

                // Loop through each row of data retrieved
                while (dataReader.Read())
                {
                    // Create new UcmRecord object for current row
                    d = new UcmRecord();

                    // Assign values from dataReader to properties of UcmRecord object
                    d.ChangeID = dataReader["ChangeID"].ToStringSafely();
                    d.ChangeCriticality = dataReader["ChangeCriticality"].ToStringSafely();
                    d.ReasonforChange = dataReader["ReasonforChange"].ToStringSafely();
                    d.DetailedReasonForChange = dataReader["DetailedReasonForChange"].ToStringSafely();
                    d.MaterialsAvailableFromSupplierDate = dataReader["MaterialsAvailableFromSupplierDate"].ToStringSafely();
                    d.SupplierConversionDate = dataReader["SupplierConversionDate"].ToStringSafely();
                    d.MainSupplierContact = dataReader["MainSupplierContact"].ToStringSafely();
                    d.IntelResponseName = dataReader["IntelResponseName"].ToStringSafely();
                    d.SupplierId = dataReader["SupplierId"].ToStringSafely();
                    d.SupplierName = dataReader["SupplierName"].ToStringSafely();
                    d.LastUpdatedOn = dataReader["LastUpdatedOn"].ToStringSafely();
                    d.IpnImpacted = dataReader["IpnImpacted"].ToStringSafely();
                    d.ChangeDescription = dataReader["ChangeDescription"].ToStringSafely();
                    d.ProcessId = dataReader["ProcessId"].ToStringSafely();
                    d.ProcessName = dataReader["ProcessName"].ToStringSafely();
                    d.XCCBBinderNumber = dataReader["XCCBBinderNumber"].ToStringSafely();
                    d.QualLocation = dataReader["QualLocation"].ToStringSafely();

                    // Add UcmRecord object to UcmRecords collection
                    data.Add(d);
                }

                // Log number of records retrieved
                log.Info("Total number of records from vw_ChangeRequest_PITT: " + data.Count);

                // Update retrieved data to database
                UpdateUcmDataToDb(data);
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
            log.Info("<-- SyncUcmData");
        }

        private void UpdateUcmDataToDb(UcmRecords data)
        {
            log.Info("--> UpdateUcmDataToDb");
            // Create a new DataTable object to hold the UcmRecord data.
            DataTable dt = new DataTable();

            // Define the structure of the DataTable by adding columns with the appropriate names and data types.

            dt.Columns.Add("ChangeID", typeof(string));
            dt.Columns.Add("ChangeCriticality", typeof(string));
            dt.Columns.Add("ReasonforChange", typeof(string));
            dt.Columns.Add("DetailedReasonForChange", typeof(string));
            dt.Columns.Add("MaterialsAvailableFromSupplierDate", typeof(string));
            dt.Columns.Add("SupplierConversionDate", typeof(string));
            dt.Columns.Add("MainSupplierContact", typeof(string));
            dt.Columns.Add("IntelResponseName", typeof(string));
            dt.Columns.Add("SupplierId", typeof(string));
            dt.Columns.Add("SupplierName", typeof(string));
            dt.Columns.Add("LastUpdatedOn", typeof(string));
            dt.Columns.Add("IpnImpacted", typeof(string));
            dt.Columns.Add("ChangeDescription", typeof(string));
            dt.Columns.Add("ProcessId", typeof(string));
            dt.Columns.Add("ProcessName", typeof(string));
            dt.Columns.Add("XCCBBinderNumber", typeof(string));
            dt.Columns.Add("QualLocation", typeof(string));

            // Loop through the UcmRecord objects in the 'data' collection, and add each record to the DataTable as a new row.
            foreach (UcmRecord d in data)
            {
                dt.Rows.Add(
                    d.ChangeID,
                    d.ChangeCriticality,
                    d.ReasonforChange,
                    d.DetailedReasonForChange,
                    d.MaterialsAvailableFromSupplierDate,
                    d.SupplierConversionDate,
                    d.MainSupplierContact,
                    d.IntelResponseName,
                    d.SupplierId,
                    d.SupplierName,
                    d.LastUpdatedOn,
                    d.IpnImpacted,
                    d.ChangeDescription,
                    d.ProcessId,
                    d.ProcessName,
                    d.XCCBBinderNumber,
                    d.QualLocation
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.UcmSP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.UcmTableType, dt);
                dataAccess.SetTimeout(Settings.SqlCommandTimeout.ToIntegerSafely());
                dataAccess.ExecuteReader();
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.InnerException);
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }

            log.Info("<-- UpdateUcmDataToDb");
        }

        public void SyncChangeCriticalityData()
        {
            log.Info("--> SyncChangeCriticalityData");
            ISqlDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                // Create data access object with UcmConnectionString and UcmQuery from settings
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.UcmConnectionString, Settings.ChangeCriticalityQuery, CommandType.Text);

                // Execute query and retrieve reader
                dataReader = dataAccess.ExecuteReader();

                // Create new UcmRecords object to store retrieved data
                ChangeCriticalities data = new ChangeCriticalities();

                // Create new UcmRecord object to store each row of data retrieved
                ChangeCriticality d = null;

                // Loop through each row of data retrieved
                while (dataReader.Read())
                {
                    // Create new UcmRecord object for current row
                    d = new ChangeCriticality();

                    // Assign values from dataReader to properties of UcmRecord object
                    d.Id = dataReader["Id"].ToStringSafely();
                    d.Name = dataReader["Name"].ToStringSafely();
                    d.Active = dataReader["Active"].ToStringSafely();
                    d.ShortName = dataReader["ShortName"].ToStringSafely();
                   
                    // Add UcmRecord object to UcmRecords collection
                    data.Add(d);
                }

                // Log number of records retrieved
                log.Info("Total number of records from ChangeCriticality: " + data.Count);

                // Update retrieved data to database
                UpdateChangeCriticalityToDb(data);
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
            log.Info("<-- SyncChangeCriticalityData");
        }
        private void UpdateChangeCriticalityToDb(ChangeCriticalities data)
        {
            log.Info("--> UpdateChangeCriticalityToDb");
            // Create a new DataTable object to hold the UcmRecord data.
            DataTable dt = new DataTable();

            // Define the structure of the DataTable by adding columns with the appropriate names and data types.

            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Active", typeof(string));
            dt.Columns.Add("ShortName", typeof(string));
           
            // Loop through the UcmRecord objects in the 'data' collection, and add each record to the DataTable as a new row.
            foreach (ChangeCriticality d in data)
            {
                dt.Rows.Add(
                    d.Id,
                    d.Name,
                    d.Active,
                    d.ShortName
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.ChangeCriticalitySP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.ChangeCriticalityTableType, dt);
                dataAccess.ExecuteReader();
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.InnerException);
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }

            log.Info("<-- UpdateChangeCriticalityToDb");
        }

        public void SyncDetailedChangeReasonData()
        {
            log.Info("--> SyncDetailedChangeReasonData");
            ISqlDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                // Create data access object with UcmConnectionString and UcmQuery from settings
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.UcmConnectionString, Settings.DetailedChangeReasonQuery, CommandType.Text);

                // Execute query and retrieve reader
                dataReader = dataAccess.ExecuteReader();

                // Create new UcmRecords object to store retrieved data
                DetailedChangeReasons data = new DetailedChangeReasons();

                // Create new UcmRecord object to store each row of data retrieved
                DetailedChangeReason d = null;

                // Loop through each row of data retrieved
                while (dataReader.Read())
                {
                    // Create new UcmRecord object for current row
                    d = new DetailedChangeReason();

                    // Assign values from dataReader to properties of UcmRecord object
                    d.Id = dataReader["Id"].ToStringSafely();
                    d.Name = dataReader["Name"].ToStringSafely();
                    d.Active = dataReader["Active"].ToStringSafely();

                    // Add UcmRecord object to UcmRecords collection
                    data.Add(d);
                }

                // Log number of records retrieved
                log.Info("Total number of records from DetailedChangeReason: " + data.Count);

                // Update retrieved data to database
                UpdateDetailedChangeReasonToDb(data);
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
            log.Info("<-- SyncDetailedChangeReasonData");
        }
        private void UpdateDetailedChangeReasonToDb(DetailedChangeReasons data)
        {
            log.Info("--> UpdateDetailedChangeReasonToDb");
            // Create a new DataTable object to hold the UcmRecord data.
            DataTable dt = new DataTable();

            // Define the structure of the DataTable by adding columns with the appropriate names and data types.
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Active", typeof(string));

            // Loop through the UcmRecord objects in the 'data' collection, and add each record to the DataTable as a new row.
            foreach (DetailedChangeReason d in data)
            {
                dt.Rows.Add(
                    d.Id,
                    d.Name,
                    d.Active
                );
            }

            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, Settings.DetailedChangeReasonSP);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", Settings.DetailedChangeReasonTableType, dt);
                dataAccess.ExecuteReader();
            }
            catch (Exception e)
            {
                log.Error("Caught!! SqlException: " + e.InnerException);
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataAccess.IsNotNull())
                {
                    dataAccess.Close();
                }
            }

            log.Info("<-- UpdateDetailedChangeReasonToDb");
        }

    }
}
