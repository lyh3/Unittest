using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.Shared.Validations;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Intel.MsoAuto.C3.MailService.Notification.Core {
    internal class ReadOnlyMongoDataAccess 
    {
        private readonly string _connectionString = string.Empty;
        private readonly string _dbName = string.Empty;
        public ReadOnlyMongoDataAccess(IConfiguration config) {
            Ensure.That<ArgumentNullException>(null != config, "Configuration connat be null.");
            IConfigurationSection pittDb = config.GetSection(Constants.MONGO_DB);
            string connectionString = string.Empty;
            string env = config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
            switch (env.ToUpper())
            {
                case Constants.DEV_CONFIG:
                    _dbName = pittDb.GetRequiredAppSettingsValueValidation(Constants.DATABASE_NAME_DEV);
                    connectionString = pittDb.GetRequiredAppSettingsValueValidation(Constants.CONNECTION_STRING_DEV);
                    break;
                case Constants.INT_CONFIG:
                    _dbName = pittDb.GetRequiredAppSettingsValueValidation(Constants.DATABASE_NAME_INT);
                    connectionString = pittDb.GetRequiredAppSettingsValueValidation(Constants.CONNECTION_STRING_INT);
                    break;
                case Constants.PROD_CONFIG:
                    _dbName = pittDb.GetRequiredAppSettingsValueValidation(Constants.DATABASE_NAME_PROD);
                    connectionString = pittDb.GetRequiredAppSettingsValueValidation(Constants.CONNECTION_STRING_PROD);
                    break;
                default:
                    throw new ArgumentNullException("Can't find environment value: " + config.GetSection("environment").Value);
            }
            string encrytKey = config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT_KEY);
            _connectionString = connectionString.Decrypt(encrytKey);
        }
        public IMongoCollection<T> GetMongoCollection<T>(in string collection)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_connectionString);
            settings.AllowInsecureTls = true;
            MongoClient client = new MongoClient(settings);
            IMongoDatabase db = client.GetDatabase(_dbName);

            bool collectionExists = db.ListCollectionNames().ToList().Contains(collection);

            if (collectionExists)
            {
                return db.GetCollection<T>(collection);
            }
            else
            {
                throw new Exception($"FATAL ERROR => Collection: {collection} doesnt exists!!");
            }
        }
        public IMongoCollection<T> GetMongoCollection<T>(in string collection, IClientSessionHandle clientSession)
        {
            IMongoDatabase db = clientSession.Client.GetDatabase(_dbName);
            bool collectionExists = db.ListCollectionNames().ToList().Contains(collection);
            if (collectionExists)
            {
                return db.GetCollection<T>(collection);
            }
            else
            {
                throw new Exception($"FATAL ERROR, Collection: {collection} doesnt exists!!");
            }
        }
        public IClientSessionHandle GetClientSession()
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_connectionString);
            settings.AllowInsecureTls = true;
            MongoClient client = new MongoClient(settings);
            return client.StartSession();
        }
    }
}
