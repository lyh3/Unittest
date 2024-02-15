using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Intel.MsoAuto.Shared;
using Intel.MsoAuto.Shared.Extensions;
using Microsoft.Extensions.Configuration;

namespace Intel.MsoAuto.C3.Loader.EMS.Business.Core
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

        private static string emsOrphanPOsSP = String.Empty;
        public static string EmsOrphanPOsSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(emsOrphanPOsSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    emsOrphanPOsSP = config.GetSection("IntegrationDbOptions:EmsOrphanPOsSP").Value;
                }
                return emsOrphanPOsSP;
            }
        }

        private static string emsOrphanPOsTableType = String.Empty;
        public static string EmsOrphanPOsTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(emsOrphanPOsTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    emsOrphanPOsTableType = config.GetSection("IntegrationDbOptions:EmsOrphanPOsTableType").Value;
                }
                return emsOrphanPOsTableType;
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

        private static string emsUrl = String.Empty;
        public static string EmsUrl
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(emsUrl))
                {
                    // get the application id from the ifs settings section of the web.config file
                    emsUrl = config.GetSection("EMSConnection:EmsUrl").Value;
                }
                return emsUrl;
            }
        }

        private static string needsIdsQuery = String.Empty;
        public static string NeedsIdsQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(needsIdsQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    needsIdsQuery = config.GetSection("NeedsInformation:NeedsIdsQuery").Value;
                }
                return needsIdsQuery;
            }
        }

        private static string needsUrl = String.Empty;
        public static string NeedsUrl
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(needsUrl))
                {
                    // get the application id from the ifs settings section of the web.config file
                    needsUrl = config.GetSection("NeedsInformation:NeedsUrl").Value;
                }
                return needsUrl;
            }
        }
    }
}
