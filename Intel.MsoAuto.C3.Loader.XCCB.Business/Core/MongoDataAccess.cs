using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.XCCB.Business.Core
{
    public class MongoDataAccess
    {
        private readonly string? _connectionString;
        private readonly string? _db;
        public MongoDataAccess()
        {
            _connectionString = Settings.DbConnectionString;
            _db = Settings.DbName;
        }

        public IMongoCollection<T> GetMongoCollection<T>(in string collection)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_connectionString);
            settings.AllowInsecureTls = true;
            MongoClient client = new MongoClient(settings);
            IMongoDatabase db = client.GetDatabase(_db);

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
    }
}
