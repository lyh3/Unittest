﻿
using Microsoft.Extensions.Configuration;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.Shared.Security;
using System.Configuration;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Core
{
    public class Settings
    {
        private static IConfiguration config;
        private static string encryptionKey;
        private static IConfiguration? _roles;
        private static string c3ConnectionString = String.Empty;
        private static string siteMappingSP = String.Empty;
        private static string dbNameDev = String.Empty;
        private static string dbConnectionStringDev = String.Empty;
        private static string dbNameInt = String.Empty;
        private static string dbConnectionStringInt = String.Empty;
        private static string projectCriticalitySp = String.Empty;
        private static string ucmTableDiffSp = String.Empty;
        private static string detailedChangeReaonsSp = String.Empty;
        private static string env = String.Empty;
        private static string globalSuppliersSp = String.Empty;
        private static string workerServiceApiUrl = String.Empty;

        public Settings(IConfiguration configuration, string key)
        {
            encryptionKey = key;
            config = configuration;
            SetRolesByEnviroment(config);
        }
        public static IConfiguration? Roles => _roles;

        public static string C3ConnectionString
        {
            get
            {
                if (String.IsNullOrWhiteSpace(c3ConnectionString))
                {
                    switch (config.GetSection("enviroment").Value)
                    {
                        case "dev":
                            c3ConnectionString = config.GetSection("SQL:ConnectionStringDev").Value;
                            break;
                        case "int":
                        case "prod":
                            c3ConnectionString = config.GetSection("SQL:ConnectionStringInt").Value;
                            break;
                        default:
                            break;
                    }

     
                }
                return c3ConnectionString;
            }
        }

        public static string SiteMappingSP
        {
            get
            {

                if (String.IsNullOrWhiteSpace(siteMappingSP))
                {
                    siteMappingSP = config.GetSection("SQL:SiteMappingsTableSP").Value;

                }
                return siteMappingSP;
            }
        }

        public static string DbNameDev
        {
            get
            {
                if (String.IsNullOrWhiteSpace(dbNameDev))
                {
                    dbNameDev = config.GetSection("MongoDb:DatabaseNameDev").Value;

                }
                return dbNameDev;
            }
        }

        public static string DbConnectionStringDev
        {
            get
            {
                if (String.IsNullOrWhiteSpace(dbConnectionStringDev))
                {
                    dbConnectionStringDev = DecryptStrings(config.GetSection("MongoDb:ConnectionStringDev").Value);

                }
                return dbConnectionStringDev;
            }
        }

        public static string DbNameInt
        {
            get
            {
                if (String.IsNullOrWhiteSpace(dbNameInt))
                {
                    dbNameInt = config.GetSection("MongoDb:DatbaseNameInt").Value;

                }
                return dbNameInt;
            }
        }

        public static string DbConnectionStringInt
        {
            get
            {
                if (String.IsNullOrWhiteSpace(dbConnectionStringInt))
                {
                    dbConnectionStringInt = DecryptStrings(config.GetSection("MongoDb:ConnectionStringInt").Value);

                }
                return dbConnectionStringInt;
            }
        }

        public static string ProjectCriticalitySp
        {
            get
            {
                if (String.IsNullOrWhiteSpace(projectCriticalitySp))
                {
                    projectCriticalitySp = config.GetSection("SQL:ProjectCriticalitySP").Value;

                }
                return projectCriticalitySp;
            }
        }

        public static string UcmTableDiffSp
        {
            get
            {
                if (String.IsNullOrWhiteSpace(ucmTableDiffSp))
                {
                    ucmTableDiffSp = config.GetSection("SQL:UcmTableDiffSP").Value;

                }
                return ucmTableDiffSp;
            }
        }

        public static string DetailedChangeReaonsSp
        {
            get
            {
                if (String.IsNullOrWhiteSpace(detailedChangeReaonsSp))
                {
                    detailedChangeReaonsSp = config.GetSection("SQL:DetailedChangeReaonsSP").Value;

                }
                return detailedChangeReaonsSp;
            }
        }
        public static string Env
        {
            get
            {
                if (String.IsNullOrWhiteSpace(env))
                {
                    env = config.GetSection("enviroment").Value;

                }
                return env;
            }
        }

        public static string GlobalSuppliersSp
        {
            get
            {
                if (String.IsNullOrWhiteSpace(globalSuppliersSp))
                {
                    globalSuppliersSp = config.GetSection("SQL:GlobalSuppliersSP").Value;

                }
                return globalSuppliersSp;
            }
        }
        
        public static string WorkerServiceApiUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(workerServiceApiUrl))
                {
                    workerServiceApiUrl = config.GetSection("WorkerService:ApiUrl").Value;

                }
                return workerServiceApiUrl;
            }
        }

        public static string DecryptStrings(string content)
        {
            Cryptographer.CryptographyType ct = Cryptographer.CryptographyType.CBC;
            return content.Decrypt(encryptionKey, ct);

        }
        private void SetRolesByEnviroment(IConfiguration configuration)
        {
            string? environment = configuration.GetSection("Enviroment").Value;

            try
            {
                if (environment == "prod")
                    _roles = configuration.GetSection("Roles").GetSection("Production");
                else
                    _roles = configuration.GetSection("Roles").GetSection("Development");
            }
            catch
            {
                throw new Exception("Unable to get or set application roles.");
            }
        }
    }
}
