
using Microsoft.Extensions.Configuration;
using Intel.MsoAuto.C3.Loader.XCCB.Business.Entities;
using Intel.MsoAuto.Shared.Security;
using System.Configuration;

namespace Intel.MsoAuto.C3.Loader.XCCB.Business.Core
{
    public class Settings
    {
        private static IConfiguration config;
        private static string encryptionKey;
        public static IConfiguration Config => config;
        public Settings(IConfiguration configuration, string key)
        {
            encryptionKey = key;
            config = configuration;
        }

        private static string dbName = String.Empty;
        public static string DbName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(dbName))
                {
                    dbName = config.GetSection("MongoDb:DatabaseName").Value;

                }
                return dbName;
            }
        }

        private static string dbConnectionString = String.Empty;
        public static string DbConnectionString
        {
            get
            {
                if (String.IsNullOrWhiteSpace(dbConnectionString))
                {
                    dbConnectionString = decryptStrings(config.GetSection("MongoDb:ConnectionString").Value);

                }
                return dbConnectionString;
            }
        }

        private static string xccbServiceApiUrl = String.Empty;
        public static string XccbServiceApiUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(xccbServiceApiUrl))
                {
                    xccbServiceApiUrl = config.GetSection("XccbService:ApiUrl").Value;

                }
                return xccbServiceApiUrl;
            }
        }
        private static string xccbCollectionName = String.Empty;
        public static string XccbCollectionName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(xccbCollectionName))
                {
                    xccbCollectionName = config.GetSection("MongoDb:XccbCollectionName").Value;

                }
                return xccbCollectionName;
            }
        }

        public static string decryptStrings(string content)
        {
            Cryptographer.CryptographyType ct = Cryptographer.CryptographyType.CBC;
            return content.Decrypt(encryptionKey, ct); ;
        }
    }
}
