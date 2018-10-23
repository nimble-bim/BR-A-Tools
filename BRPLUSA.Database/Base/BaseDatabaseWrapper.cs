using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BRPLUSA.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BRPLUSA.Database.Base
{
    public abstract class BaseDatabaseWrapper<T> : IDisposable where T : IEntity
    {
        private readonly string _dbName;
        private readonly string _dbLocalLocation;
        private readonly string _dbServerLocation;
        public IMongoDatabase Database { get; set; }
        public IMongoClient Client { get; set; }

        public BaseDatabaseWrapper()
        {
            var type = typeof(T);
            _dbName = type.Name.ToLower() + "db";
            Initialize();
        }

        public BaseDatabaseWrapper(string dbLocation)
        {
            _dbLocalLocation = dbLocation;
        }

        private void Initialize()
        {
            if (Client == null)
            {
                Client = _dbServerLocation == null
                    ? new MongoClient("mongodb://localhost:27017")
                    : new MongoClient(_dbServerLocation);
            }

            Database = _dbName == null 
                ? Client.GetDatabase("brplusa") 
                : Client.GetDatabase(_dbName);
        }

        public IMongoCollection<T> CreateTableIfNotExists(string tableName)
        {
            return CreateTableIfNotExists<T>(tableName);
        }

        public IMongoCollection<T1> CreateTableIfNotExists<T1>(string tableName)
        {
            if (!TableExists(tableName))
                Database.CreateCollection(tableName);

            return Database.GetCollection<T1>(tableName);
        }

        public bool TableExists(string tableName)
        {
            var filter = new BsonDocument("name", tableName);

            var dbs = Database.ListCollections(new ListCollectionsOptions { Filter = filter });

            return dbs.Any();
        }

        public Expression<Func<T, bool>> IsElement(T elem)
        {
            return (e => elem.InternalId == e.InternalId);
        }

        public async Task<T> FindElement(string tableName, T element)
        {
            T dbElem = default(T);

            try
            {
                var table = Database.GetCollection<T>(tableName);
                var doc = await table.FindAsync(e => element.InternalId == e.InternalId);
                dbElem = (T)doc;
            }

            catch (Exception e)
            {
                Console.WriteLine($"unable to fidn document: {e.Message}");
            }

            Console.WriteLine("found document");
            return dbElem;
        }

        public async Task<bool> AddElement(string tableName, T element)
        {
            try
            {
                var table = Database.GetCollection<T>(tableName);
                await table.InsertOneAsync(element);
            }

            catch(Exception e)
            {
                Console.WriteLine($"unable to write document: {e.Message}");
                return false;
            }

            Console.WriteLine("wrote document");
            return true;
        }

        public async Task<bool> RemoveElement(string tableName, T element)
        {
            DeleteResult result = null;

            try
            {
                var table = Database.GetCollection<T>(tableName);
                result = await table.DeleteOneAsync(e => element.InternalId == e.InternalId);
            }

            catch (Exception e)
            {
                Console.WriteLine($"unable to delete document: {e.Message}");
                return false;
            }

            return result.IsAcknowledged;
        }

        public void Dispose()
        {
            if (Client != null)
                Client = null;
        }
    }
}