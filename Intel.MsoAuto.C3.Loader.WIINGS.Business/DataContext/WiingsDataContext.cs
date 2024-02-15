//using Intel.MsoAuto.C3.Loader.WSPW.Business.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Intel.MsoAuto.Shared;
using System.IO;
using System.Security.Policy;
using System.Diagnostics;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.DataAccess;
using Intel.MsoAuto.DataAccess.Sql;
using Intel.MsoAuto.DataAccess.Teradata;
using System.Reflection;
using Intel.MsoAuto.C3.Loader.WIINGS.Business.Core;
using Intel.MsoAuto.C3.Loader.WIINGS.Business.Entities;
using log4net;
using System.Reflection.PortableExecutable;
using System.Data.Common;
using Teradata.Client.Provider;

namespace Intel.MsoAuto.C3.Loader.WIINGS.Business.DataContext
{
    public class WiingsDataContext : IWiingsDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WiingsDataContext()
        {

        }

        public void SyncIpnPriceData()
        {
            log.Info("--> SyncIpnPriceData");
            ITeradataDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                log.Info("Connecting...");
                //Only sys_msoac3 has access to this data source
                dataAccess = new DataAccessFactory().CreateTeraDataAccess("Data Source = TDPRD1.intel.com;Integrated Security=true;", Settings.IpnPriceBySiteQuery, CommandType.Text);

                log.Info("Connected...");
                dataReader = dataAccess.ExecuteReader();
                int fCount = dataReader.FieldCount;

                IpnPrices data = new IpnPrices();
                IpnPrice d = null;
                while (dataReader.Read())
                {
                    d = new IpnPrice();
                    d.Ipn = dataReader["IPN"].ToStringSafely();
                    d.Site = dataReader["Intel Site"].ToStringSafely();
                    d.AvgSiteNewBuyPrice = dataReader["Avg Site New Buy Price"].ToStringSafely();
                    d.AvgSiteRepairPrice = dataReader["Avg Site Repair Price"].ToStringSafely();

                    data.Add(d);
                }
                log.Info("Total number of records for IpnPrices: " + data.Count);
                UpdateIpnPriceData(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                if (dataReader.IsNotNull())
                    dataReader.Close();
                if (dataAccess.IsNotNull())
                    dataAccess.Close();
            }
            log.Info("<-- SyncIpnPriceData");
        }

        public void SyncAltIpnAndQuantityData()
        {
            log.Info("--> SyncAltIpnAndQuantityData");
            ITeradataDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                log.Info("Connecting...");
                //Only sys_msoac3 has access to this data source
                dataAccess = new DataAccessFactory().CreateTeraDataAccess("Data Source = TDPRD1.intel.com;Integrated Security=true;", Settings.AltIpnAndQtyQuery, CommandType.Text);
                log.Info("Connected...");
                dataReader = dataAccess.ExecuteReader();
                int fCount = dataReader.FieldCount;

                AlternateIpnsAndQuantities data = new AlternateIpnsAndQuantities();
                AlternateIpnAndQuantity d = null;
                while (dataReader.Read())
                {
                    d = new AlternateIpnAndQuantity();
                    d.AltIpn = dataReader["AltIpn"].ToStringSafely();
                    d.PrimaryIpn = dataReader["PrimaryIpn"].ToStringSafely();
                    d.AltIpnDesc = dataReader["AltIpnDesc"].ToStringSafely();
                    d.SiteOfAltIpn = dataReader["SiteOfAltIpn"].ToStringSafely();
                    d.AvlQty = dataReader["AvlQty"].ToStringSafely();
                    d.Last90DaySiteIssues = dataReader["Last90DaySiteIssues"].ToStringSafely();

                    data.Add(d);
                }
                log.Info("Total number of records for Alt Ipns And Quantities: " + data.Count);
                UpdateAltIpnAndQuantityData(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                if (dataReader.IsNotNull())
                    dataReader.Close();
                if (dataAccess.IsNotNull())
                    dataAccess.Close();
            }
            log.Info("<-- SyncAltIpnAndQuantityData");
        }

        public void SyncGlobalSupplierData()
        {
            log.Info("--> SyncGlobalSupplierData");
            ITeradataDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                log.Info("Connecting...");
                //Only sys_msoac3 has access to this data source
                dataAccess = new DataAccessFactory().CreateTeraDataAccess("Data Source = TDPRD1.intel.com;Integrated Security=true;", Settings.GlobalSuppliersQuery, CommandType.Text);
                log.Info("Connected...");
                dataReader = dataAccess.ExecuteReader();
                GlobalSuppliers data = new GlobalSuppliers();
                GlobalSupplier d = null;
                while (dataReader.Read())
                {
                    d = new GlobalSupplier();
                    d.BusinessPartyId = dataReader["busns_org_prty_id"].ToStringSafely();
                    d.BusinessOrgName = dataReader["busns_org_nm"].ToStringSafely();
                    d.ParentBusinessPartyId = dataReader["parnt_busns_org_prty_id"].ToStringSafely();
                    d.ParentBusinessOrgName = dataReader["parnt_busns_org_nm"].ToStringSafely();
                    d.GlobalBusinessPartyId = dataReader["glbl_busns_org_prty_id"].ToStringSafely();
                    d.GlobalBusinessOrgName = dataReader["glbl_busns_org_nm"].ToStringSafely();

                    data.Add(d);
                }
                log.Info("Total number of records for GlobalSuppliers: " + data.Count);
                UpdateGlobalSuppliersData(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                if (dataReader.IsNotNull())
                    dataReader.Close();
                if (dataAccess.IsNotNull())
                    dataAccess.Close();
            }
            log.Info("<-- SyncGlobalSupplierData");
        }

        public void SyncSiteReceiptQuantityData()
        {
            log.Info("--> SyncSiteReceiptQuantityData");
            ITeradataDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                log.Info("Connecting...");
                //Only sys_msoac3 has access to this data source
                dataAccess = new DataAccessFactory().CreateTeraDataAccess("Data Source = TDPRD1.intel.com;Integrated Security=true;", Settings.SiteReceiptQuantityQuery, CommandType.Text);
                log.Info("Connected...");
                dataReader = dataAccess.ExecuteReader();
                SiteReceiptQuantities data = new SiteReceiptQuantities();
                SiteReceiptQuantity d = null;
                while (dataReader.Read())
                {
                    d = new SiteReceiptQuantity();
                    d.Ipn = dataReader["IPN"].ToStringSafely();
                    d.Site = dataReader["Site"].ToStringSafely();
                    d.ReceiptQuantity = dataReader["Receipt_Qty"].ToStringSafely();
                    d.AsOfSourceTimeStamp = dataReader["AsOfSourceTimeStamp"].ToStringSafely();
                    
                    data.Add(d);
                }
                log.Info("Total number of records for SiteReceiptQuantity: " + data.Count);
                UpdateSiteReceiptQuantityData(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                if (dataReader.IsNotNull())
                    dataReader.Close();
                if (dataAccess.IsNotNull())
                    dataAccess.Close();
            }
            log.Info("<-- SyncSiteReceiptQuantityData");
        }
        public void SyncSiteMaxQuantityData()
        {
            log.Info("--> SyncSiteMaxQuantityData");
            ITeradataDataAccess dataAccess = null;
            IDataReader dataReader = null;

            try
            {
                log.Info("Connecting...");
                //Only sys_msoac3 has access to this data source
                dataAccess = new DataAccessFactory().CreateTeraDataAccess("Data Source = TDPRD1.intel.com;Integrated Security=true;", Settings.SiteMaxQuantityQuery, CommandType.Text);
                log.Info("Connected...");
                dataReader = dataAccess.ExecuteReader();
                SiteMaxQuantities data = new SiteMaxQuantities();
                SiteMaxQuantity d = null;
                while (dataReader.Read())
                {
                    d = new SiteMaxQuantity();
                    d.Ipn = dataReader["Ipn"].ToStringSafely();
                    d.Site = dataReader["Site"].ToStringSafely();
                    d.QuantityAvailable = dataReader["QuantityAvailable"].ToStringSafely();
                    d.SiteMaxQty = dataReader["SiteMaxQty"].ToStringSafely();
                    d.OnHandQuantity = dataReader["OnHandQuantity"].ToStringSafely();
                    d.SiteDoi = dataReader["SiteDoi"].ToStringSafely();
                    d.CountOfStockrooms = dataReader["CountOfStockrooms"].ToStringSafely();

                    data.Add(d);
                }
                log.Info("Total number of records for SiteMaxQuantity: " + data.Count);
                UpdateSiteMaxQuantityData(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                if (dataReader.IsNotNull())
                    dataReader.Close();
                if (dataAccess.IsNotNull())
                    dataAccess.Close();
            }
            log.Info("<-- SyncSiteMaxQuantityData");
        }

        private void updateData(string storedProc, string tableType, DataTable dt)
        {
            ISqlDataAccess dataAccess = null;

            try
            {
                dataAccess = new DataAccessFactory().CreateSqlDataAccess(Settings.IntegrationDbConnString, storedProc);
                //dataAccess.AddInputParameter("@inputTable", dt);
                dataAccess.AddTableValueParameter("@inputTable", tableType, dt);
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
        }

        private void UpdateIpnPriceData(IpnPrices data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IPN", typeof(string));
            dt.Columns.Add("IntelSite", typeof(string));
            dt.Columns.Add("AvgSiteNewBuyPrice", typeof(string));
            dt.Columns.Add("AvgSiteRepairPrice", typeof(string));

            foreach (IpnPrice d in data)
            {
                dt.Rows.Add(
                    d.Ipn,
                    d.Site,
                    d.AvgSiteNewBuyPrice,
                    d.AvgSiteRepairPrice
                );
            }

            updateData(Settings.IpnPriceBySiteSP, Settings.IpnPriceBySiteTableType, dt);
        }

        private void UpdateAltIpnAndQuantityData(AlternateIpnsAndQuantities data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AltIpn", typeof(string));
            dt.Columns.Add("PrimaryIpn", typeof(string));
            dt.Columns.Add("AltIpnDesc", typeof(string));
            dt.Columns.Add("SiteOfAltIpn", typeof(string));
            dt.Columns.Add("AvlQty", typeof(string));
            dt.Columns.Add("Last90DaySiteIssues", typeof(string));

            foreach (AlternateIpnAndQuantity d in data)
            {
                dt.Rows.Add(
                    d.AltIpn,
                    d.PrimaryIpn,
                    d.AltIpnDesc,
                    d.SiteOfAltIpn,
                    d.AvlQty,
                    d.Last90DaySiteIssues
                );
            }

            updateData(Settings.AltIpnAndQtySP, Settings.AltIpnAndQtyTableType, dt);
        }

        private void UpdateGlobalSuppliersData(GlobalSuppliers data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BusinessPartyId", typeof(string));
            dt.Columns.Add("BusinessOrgName", typeof(string));
            dt.Columns.Add("ParentBusinessPartyId", typeof(string));
            dt.Columns.Add("ParentBusinessOrgName", typeof(string));
            dt.Columns.Add("GlobalBusinessPartyId", typeof(string));
            dt.Columns.Add("GlobalBusinessOrgName", typeof(string));

            foreach (GlobalSupplier d in data)
            {
                dt.Rows.Add(
                    d.BusinessPartyId,
                    d.BusinessOrgName,
                    d.ParentBusinessPartyId,
                    d.ParentBusinessOrgName,
                    d.GlobalBusinessPartyId,
                    d.GlobalBusinessOrgName
                );
            }

            updateData(Settings.GlobalSuppliersSP, Settings.GlobalSuppliersTableType, dt);
        }
        private void UpdateSiteReceiptQuantityData(SiteReceiptQuantities data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Ipn", typeof(string));
            dt.Columns.Add("Site", typeof(string));
            dt.Columns.Add("ReceiptQuantity", typeof(string));
            dt.Columns.Add("AsOfSourceTimeStamp", typeof(string));
            
            foreach (SiteReceiptQuantity d in data)
            {
                dt.Rows.Add(
                    d.Ipn,
                    d.Site,
                    d.ReceiptQuantity,
                    d.AsOfSourceTimeStamp
                );
            }

            updateData(Settings.SiteReceiptQuantitySP, Settings.SiteReceiptQuantityTableType, dt);
        }
        private void UpdateSiteMaxQuantityData(SiteMaxQuantities data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Ipn", typeof(string));
            dt.Columns.Add("Site", typeof(string));
            dt.Columns.Add("QuantityAvailable", typeof(string));
            dt.Columns.Add("SiteMaxQty", typeof(string));
            dt.Columns.Add("OnHandQuantity", typeof(string));
            dt.Columns.Add("SiteDoi", typeof(string));
            dt.Columns.Add("CountOfStockrooms", typeof(string));

            foreach (SiteMaxQuantity d in data)
            {
                dt.Rows.Add(
                    d.Ipn,
                    d.Site,
                    d.QuantityAvailable,
                    d.SiteMaxQty,
                    d.OnHandQuantity,
                    d.SiteDoi,
                    d.CountOfStockrooms
                );
            }

            updateData(Settings.SiteMaxQuantitySP, Settings.SiteMaxQuantityTableType, dt);
        }

    }
}
