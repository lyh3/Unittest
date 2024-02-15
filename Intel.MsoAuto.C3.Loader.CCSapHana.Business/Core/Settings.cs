using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.Core
{
    public static class Settings
    {
        private static IConfigurationBuilder _builder;
        private static IConfiguration _config;
        private static string _connectionStringPreProd = String.Empty;
        private static string _ceidQuery = String.Empty;
        private static string _integrationDbConnString = String.Empty;
        private static string _ceidSP = String.Empty;
        public static IConfiguration Config => _config;

        static Settings()
        {
            _builder = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json", false, true);
            _config = _builder.Build();
        }

        public static string ConnectionStringPreProd
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_connectionStringPreProd))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _connectionStringPreProd = _config.GetSection("SapDbOptions:ConnectionStringPreProd").Value;
                }
                return _connectionStringPreProd;
            }
        }

        public static string CeidQuery
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_ceidQuery))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _ceidQuery = _config.GetSection("SapDbOptions:CopyExactIdentifierQuery").Value;
                }
                return _ceidQuery;
            }
        }

        public static string IntegrationDbConnString
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_integrationDbConnString))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _integrationDbConnString = _config.GetSection("IntegrationDbOptions:ConnectionString").Value;
                }
                return _integrationDbConnString;
            }
        }

        public static string CeidSP
        {
            get
            {
                // if the application id is null
                if (String.IsNullOrWhiteSpace(_ceidSP))
                {
                    // get the application id from the ifs settings section of the web.config file
                    _ceidSP = _config.GetSection("IntegrationDbOptions:CopyExactIdentifierSP").Value;
                }
                return _ceidSP;
            }
        }

    }
}
