using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Intel.MsoAuto.Shared;
using Intel.MsoAuto.Shared.Extensions;
using Microsoft.Extensions.Configuration;

namespace Intel.MsoAuto.C3.Loader.SQL.Business.Core
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
        private static string gridConnectionString = String.Empty;
        public static string GridConnectionString
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(gridConnectionString))
                {
                    // get the application id from the ifs settings section of the web.config file
                    gridConnectionString = config.GetSection("GridConnection:ConnectionString").Value;
                }
                return gridConnectionString;
            }
        }
        private static string ucmConnectionString = String.Empty;
        public static string UcmConnectionString
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(ucmConnectionString))
                {
                    // get the application id from the ifs settings section of the web.config file
                    ucmConnectionString = config.GetSection("UcmConnection:ConnectionString").Value;
                }
                return ucmConnectionString;
            }
        }

        private static string supplierContactQuery = String.Empty;
        public static string SupplierContactQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(supplierContactQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    supplierContactQuery = config.GetSection("GridConnection:SupplierContactQuery").Value;
                }
                return supplierContactQuery;
            }
        }

        private static string supplierIntelContactQuery = String.Empty;
        public static string SupplierIntelContactQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(supplierIntelContactQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    supplierIntelContactQuery = config.GetSection("GridConnection:SupplierIntelContactQuery").Value;
                }
                return supplierIntelContactQuery;
            }
        }
        private static string ucmQuery = String.Empty;
        public static string UcmQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(ucmQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    ucmQuery = config.GetSection("UcmConnection:UcmQuery").Value;
                }
                return ucmQuery;
            }
        }

        private static string changeCriticalityQuery = String.Empty;
        public static string ChangeCriticalityQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(changeCriticalityQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    changeCriticalityQuery = config.GetSection("UcmConnection:ChangeCriticalityQuery").Value;
                }
                return changeCriticalityQuery;
            }
        }
        private static string detailedChangeReasonQuery = String.Empty;
        public static string DetailedChangeReasonQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(detailedChangeReasonQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    detailedChangeReasonQuery = config.GetSection("UcmConnection:DetailedChangeReasonQuery").Value;
                }
                return detailedChangeReasonQuery;
            }
        }

        private static string supplierContactTableType = String.Empty;
        public static string SupplierContactTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(supplierContactTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    supplierContactTableType = config.GetSection("IntegrationDbOptions:SupplierContactTableType").Value;
                }
                return supplierContactTableType;
            }
        }

        private static string supplierIntelContactTableType = String.Empty;
        public static string SupplierIntelContactTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(supplierIntelContactTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    supplierIntelContactTableType = config.GetSection("IntegrationDbOptions:SupplierIntelContactTableType").Value;
                }
                return supplierIntelContactTableType;
            }
        }
        private static string supplierContactSP = String.Empty;
        public static string SupplierContactSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(supplierContactSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    supplierContactSP = config.GetSection("IntegrationDbOptions:SupplierContactSP").Value;
                }
                return supplierContactSP;
            }
        }
        private static string supplierIntelContactSP = String.Empty;
        public static string SupplierIntelContactSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(supplierIntelContactSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    supplierIntelContactSP = config.GetSection("IntegrationDbOptions:SupplierIntelContactSP").Value;
                }
                return supplierIntelContactSP;
            }
        }
        private static string ucmTableType = String.Empty;
        public static string UcmTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(ucmTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    ucmTableType = config.GetSection("IntegrationDbOptions:UcmTableType").Value;
                }
                return ucmTableType;
            }
        }
        private static string ucmSP = String.Empty;
        public static string UcmSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(ucmSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    ucmSP = config.GetSection("IntegrationDbOptions:UcmSP").Value;
                }
                return ucmSP;
            }
        }

        private static string changeCriticalityTableType = String.Empty;
        public static string ChangeCriticalityTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(changeCriticalityTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    changeCriticalityTableType = config.GetSection("IntegrationDbOptions:ChangeCriticalityTableType").Value;
                }
                return changeCriticalityTableType;
            }
        }
        private static string changeCriticalitySP = String.Empty;
        public static string ChangeCriticalitySP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(changeCriticalitySP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    changeCriticalitySP = config.GetSection("IntegrationDbOptions:ChangeCriticalitySP").Value;
                }
                return changeCriticalitySP;
            }
        }

        private static string detailedChangeReasonTableType = String.Empty;
        public static string DetailedChangeReasonTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(detailedChangeReasonTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    detailedChangeReasonTableType = config.GetSection("IntegrationDbOptions:DetailedChangeReasonTableType").Value;
                }
                return detailedChangeReasonTableType;
            }
        }
        private static string detailedChangeReasonSP = String.Empty;
        public static string DetailedChangeReasonSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(detailedChangeReasonSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    detailedChangeReasonSP = config.GetSection("IntegrationDbOptions:DetailedChangeReasonSP").Value;
                }
                return detailedChangeReasonSP;
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

        private static string sqlCommandTimeout = String.Empty;
        public static string SqlCommandTimeout
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(sqlCommandTimeout))
                {
                    // get the application id from the ifs settings section of the web.config file
                    sqlCommandTimeout = config.GetSection("IntegrationDbOptions:SqlCommandTimeout").Value;
                }
                return sqlCommandTimeout;
            }
        }
    }
}
