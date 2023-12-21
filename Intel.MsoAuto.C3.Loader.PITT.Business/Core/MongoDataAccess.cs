using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Core
{
    public class MongoDataAccess
    {
        private readonly string? _connectionString;
        private readonly string? _db;
        public MongoDataAccess()
        {

            switch (Settings.Env)
            {
                case "dev":
                    _connectionString = Settings.DbConnectionStringDev;
                    _db = Settings.DbNameDev;
                    break;
                case "int":
                    _connectionString = Settings.DbConnectionStringInt;
                    _db = Settings.DbNameInt;
                    break;
                default: 
                    throw new ArgumentException("No env variable was supplied");
         
            }
        
        }

        public IMongoCollection<T> GetMongoCollection<T>(in string collection)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_connectionString);
            settings.AllowInsecureTls = true;
            MongoClient client = new MongoClient(settings);
            IMongoDatabase db = client.GetDatabase(_db);

            bool collectionExists = db.ListCollectionNames().ToList().Contains(collection);

     
            return db.GetCollection<T>(collection);
           
        }

        public IMongoCollection<T> GetMongoCollection<T>(in string collection, IClientSessionHandle clientSession)
        {
            IMongoDatabase db = clientSession.Client.GetDatabase(_db);

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

        public IClientSessionHandle GetClientSession()
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_connectionString);
            settings.AllowInsecureTls = true;
            MongoClient client = new MongoClient(settings);

            return client.StartSession();
        }
    }
}
