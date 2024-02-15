using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Intel.MsoAuto.Shared;
using Intel.MsoAuto.Shared.Extensions;
using Microsoft.Extensions.Configuration;

namespace Intel.MsoAuto.C3.Loader.WIINGS.Business.Core
{
    public static class Settings
    {
        /// <summary>
        /// Holds the connection string
        /// </summary>
        //private static string c3CommonConnectionString = String.Empty;
        private static IConfigurationBuilder builder;
        private static IConfiguration config;
        public static IConfiguration Config => config;

        static Settings()
        {
            builder = new ConfigurationBuilder()
                   .AddJsonFile("appSettings.json", false, true);
            config = builder.Build();
        }

        /// <summary>
        /// Gets the connection setting for this web application
        /// </summary>
        /// 
        private static string wiingsConnectionString = String.Empty;
        public static string WiingsConnectionString
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(wiingsConnectionString))
                {
                    // get the application id from the ifs settings section of the web.config file
                    wiingsConnectionString = config.GetSection("TeradataConnection:WIINGS").Value;
                }
                return wiingsConnectionString;
            }
        }

        private static string ipnPriceBySiteQuery = String.Empty;
        public static string AltIpnAndQtyQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(altIpnAndQtyQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    altIpnAndQtyQuery = config.GetSection("TeradataConnection:AltIpnAndQtyQuery").Value;
                }
                return altIpnAndQtyQuery;
            }
        }

        private static string ipnPriceBySiteSP = String.Empty;
        public static string IpnPriceBySiteSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(ipnPriceBySiteSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    ipnPriceBySiteSP = config.GetSection("IntegrationDbOptions:IpnPriceBySiteSP").Value;
                }
                return ipnPriceBySiteSP;
            }
        }
        private static string ipnPriceBySiteTableType = String.Empty;
        public static string IpnPriceBySiteTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(ipnPriceBySiteTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    ipnPriceBySiteTableType = config.GetSection("IntegrationDbOptions:IpnPriceBySiteTableType").Value;
                }
                return ipnPriceBySiteTableType;
            }
        }

        private static string altIpnAndQtyQuery = String.Empty;
        public static string IpnPriceBySiteQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(ipnPriceBySiteQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    ipnPriceBySiteQuery = config.GetSection("TeradataConnection:IpnPriceBySiteQuery").Value;
                }
                return ipnPriceBySiteQuery;
            }
        }

        private static string altIpnAndQtySP = String.Empty;
        public static string AltIpnAndQtySP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(altIpnAndQtySP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    altIpnAndQtySP = config.GetSection("IntegrationDbOptions:AltIpnAndQtySP").Value;
                }
                return altIpnAndQtySP;
            }
        }

        private static string altIpnAndQtyTableType = String.Empty;
        public static string AltIpnAndQtyTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(altIpnAndQtyTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    altIpnAndQtyTableType = config.GetSection("IntegrationDbOptions:AltIpnAndQtyTableType").Value;
                }
                return altIpnAndQtyTableType;
            }
        }

        private static string globalSuppliersQuery = String.Empty;
        public static string GlobalSuppliersQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(globalSuppliersQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    globalSuppliersQuery = config.GetSection("TeradataConnection:GlobalSuppliersQuery").Value;
                }
                return globalSuppliersQuery;
            }
        }

        private static string globalSuppliersSP = String.Empty;
        public static string GlobalSuppliersSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(globalSuppliersSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    globalSuppliersSP = config.GetSection("IntegrationDbOptions:GlobalSuppliersSP").Value;
                }
                return globalSuppliersSP;
            }
        }

        private static string globalSuppliersTableType = String.Empty;
        public static string GlobalSuppliersTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(globalSuppliersTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    globalSuppliersTableType = config.GetSection("IntegrationDbOptions:GlobalSuppliersTableType").Value;
                }
                return globalSuppliersTableType;
            }
        }

        private static string siteReceiptQuantityQuery = String.Empty;
        public static string SiteReceiptQuantityQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(siteReceiptQuantityQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    siteReceiptQuantityQuery = config.GetSection("TeradataConnection:SiteReceiptQuantityQuery").Value;
                }
                return siteReceiptQuantityQuery;
            }
        }

        private static string siteReceiptQuantitySP = String.Empty;
        public static string SiteReceiptQuantitySP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(siteReceiptQuantitySP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    siteReceiptQuantitySP = config.GetSection("IntegrationDbOptions:SiteReceiptQuantitySP").Value;
                }
                return siteReceiptQuantitySP;
            }
        }

        private static string siteReceiptQuantityTableType = String.Empty;
        public static string SiteReceiptQuantityTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(siteReceiptQuantityTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    siteReceiptQuantityTableType = config.GetSection("IntegrationDbOptions:SiteReceiptQuantityTableType").Value;
                }
                return siteReceiptQuantityTableType;
            }
        }

        private static string siteMaxQuantityQuery = String.Empty;
        public static string SiteMaxQuantityQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(siteMaxQuantityQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    siteMaxQuantityQuery = config.GetSection("TeradataConnection:SiteMaxQuantityQuery").Value;
                }
                return siteMaxQuantityQuery;
            }
        }

        private static string siteMaxQuantitySP = String.Empty;
        public static string SiteMaxQuantitySP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(siteMaxQuantitySP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    siteMaxQuantitySP = config.GetSection("IntegrationDbOptions:SiteMaxQuantitySP").Value;
                }
                return siteMaxQuantitySP;
            }
        }

        private static string siteMaxQuantityTableType = String.Empty;
        public static string SiteMaxQuantityTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(siteMaxQuantityTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    siteMaxQuantityTableType = config.GetSection("IntegrationDbOptions:SiteMaxQuantityTableType").Value;
                }
                return siteMaxQuantityTableType;
            }
        }


        private static string integrationDbConnString = String.Empty;
        public static string IntegrationDbConnString
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(integrationDbConnString))
                {
                    // get the application id from the ifs settings section of the web.config file
                    integrationDbConnString = config.GetSection("IntegrationDbOptions:ConnectionString").Value;
                }
                return integrationDbConnString;
            }
        }
    }
}
