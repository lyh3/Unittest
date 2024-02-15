using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Intel.MsoAuto.Shared;
using Intel.MsoAuto.Shared.Extensions;
using Microsoft.Extensions.Configuration;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.Core
{
    public static class Settings
    {
        /// <summary>
        /// Holds the connection string
        /// </summary>
        //private static string c3CommonConnectionString = String.Empty;
        private static IConfigurationBuilder builder;
        private static IConfiguration config;
        private static string _allocatedGasChemQuery = String.Empty;
        private static string _gasChemSP = String.Empty;
        private static string _allocatedSpareQuery = String.Empty;
        private static string _spareTransactionSP = String.Empty;
        private static string _parentCeidMappingQuery = String.Empty;
        private static string _parentCEIDMappingSP = String.Empty;
        private static string _parentCeidPercentWspwQuery = String.Empty;
        private static string _parentCeidPercentWspwSP = String.Empty;
        private static string _gasChemTransactionTableType = String.Empty;
        private static string _spareTransactionTableType = String.Empty;
        private static string _parentCEIDMappingTableType = String.Empty;
        private static string _parentCeidPercentWspwTableType = String.Empty;
        public static IConfiguration? Config => config;
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
        private static string wspwConnectionString = String.Empty;
        public static string WspwConnectionString
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(wspwConnectionString))
                {
                    // get the application id from the ifs settings section of the web.config file
                    wspwConnectionString = config.GetSection("ODBCConnection:WSPW").Value;
                }
                return wspwConnectionString;
            }
        }

        private static string capacityPlanCommand = String.Empty;
        public static string CapacityPlanCommand
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(capacityPlanCommand))
                {
                    // get the application id from the ifs settings section of the web.config file
                    capacityPlanCommand = config.GetSection("ODBCConnection:WaferStartLongRangeCapacityPlanning").Value;
                }
                return capacityPlanCommand;
            }
        }

        private static string capacityPlanSP = String.Empty;
        public static string CapacityPlanSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(capacityPlanSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    capacityPlanSP = config.GetSection("IntegrationDbOptions:WsCapacityPlanningSP").Value;
                }
                return capacityPlanSP;
            }
        }

        private static string capacityPlanTableType = String.Empty;
        public static string CapacityPlanTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(capacityPlanTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    capacityPlanTableType = config.GetSection("IntegrationDbOptions:WsCapacityPlanningTableType").Value;
                }
                return capacityPlanTableType;
            }
        }

        private static string demandForecastCommand = String.Empty;
        public static string DemandForecastCommand
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(demandForecastCommand))
                {
                    // get the application id from the ifs settings section of the web.config file
                    demandForecastCommand = config.GetSection("ODBCConnection:WaferStartLongRangeDemandForecast").Value;
                }
                return demandForecastCommand;
            }
        }
        private static string demandForecastSP = String.Empty;
        public static string DemandForecastSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(demandForecastSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    demandForecastSP = config.GetSection("IntegrationDbOptions:WsDemandForecastSP").Value;
                }
                return demandForecastSP;
            }
        }

        private static string demandForecastTableType = String.Empty;
        public static string DemandForecastTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(demandForecastTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    demandForecastTableType = config.GetSection("IntegrationDbOptions:WsDemandForecastTableType").Value;
                }
                return demandForecastTableType;
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
        
        public static string AllocatedGasChemQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_allocatedGasChemQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _allocatedGasChemQuery = config.GetSection("ODBCConnection:AllocatedGasChemQuery").Value;
                }
                return _allocatedGasChemQuery;
            }
        }

        public static string GasChemSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_gasChemSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _gasChemSP = config.GetSection("IntegrationDbOptions:GasChemTransactionSP").Value;
                }
                return _gasChemSP;
            }
        }
        public static string GasChemTransactionTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_gasChemTransactionTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _gasChemTransactionTableType = config.GetSection("ODBCConnection:GasChemTransactionTableType").Value;
                }
                return _gasChemTransactionTableType;
            }
        }

        public static string AllocatedSpareQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_allocatedSpareQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _allocatedSpareQuery = config.GetSection("ODBCConnection:AllocatedSpareQuery").Value;
                }
                return _allocatedSpareQuery;
            }
        }

        public static string SpareTransactionSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_spareTransactionSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _spareTransactionSP = config.GetSection("IntegrationDbOptions:SpareTransactionSP").Value;
                }
                return _spareTransactionSP;
            }
        }
        public static string SpareTransactionTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_spareTransactionTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _spareTransactionTableType = config.GetSection("ODBCConnection:SpareTransactionTableType").Value;
                }
                return _spareTransactionTableType;
            }
        }

        public static string ParentCeidMappingQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_parentCeidMappingQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _parentCeidMappingQuery = config.GetSection("ODBCConnection:ParentCeidMappingQuery").Value;
                }
                return _parentCeidMappingQuery;
            }
        }

        public static string ParentCEIDMappingSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_parentCEIDMappingSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _parentCEIDMappingSP = config.GetSection("IntegrationDbOptions:ParentCEIDMappingSP").Value;
                }
                return _parentCEIDMappingSP;
            }
        }
        public static string ParentCEIDMappingTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_parentCEIDMappingTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _parentCEIDMappingTableType = config.GetSection("ODBCConnection:ParentCEIDMappingTableType").Value;
                }
                return _parentCEIDMappingTableType;
            }
        }

        public static string ParentCeidPercentWspwQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_parentCeidPercentWspwQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _parentCeidPercentWspwQuery = config.GetSection("ODBCConnection:ParentCeidPercentWspwQuery").Value;
                }
                return _parentCeidPercentWspwQuery;
            }
        }

        public static string ParentCeidPercentWspwSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_parentCeidPercentWspwSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _parentCeidPercentWspwSP = config.GetSection("IntegrationDbOptions:ParentCeidPercentWspwSP").Value;
                }
                return _parentCeidPercentWspwSP;
            }
        }
        public static string ParentCeidPercentWspwTableType
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_parentCeidPercentWspwTableType))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _parentCeidPercentWspwTableType = config.GetSection("ODBCConnection:ParentCeidPercentWspwTableType").Value;
                }
                return _parentCeidPercentWspwTableType;
            }
        }
    }
}
