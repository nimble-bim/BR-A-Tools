using BRPLUSA.Domain.Entities;
using BRPLUSA.Domain.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BRPLUSA.Revit.Client.Updaters
{
    public abstract class BaseDatabaseWrapper<T> : IDisposable where T : IEntity
    {
        protected IMongoDatabase _db;
        protected IMongoClient _client;
        private readonly string _dbName;
        private readonly string _dbLocalLocation;
        private readonly string _dbServerLocation;

        public BaseDatabaseWrapper()
        {
            Initialize();
        }

        public BaseDatabaseWrapper(string dbLocation)
        {
            _dbName = typeof(T).ToString();
            _dbLocalLocation = dbLocation;
        }

        private void Initialize()
        {
            if (_client == null)
            {
                _client = _dbServerLocation == null
                    ? new MongoClient("mongodb://localhost:27017")
                    : new MongoClient(_dbServerLocation);
            }

            _db = _dbName == null 
                ? _client.GetDatabase("brplusa") 
                : _client.GetDatabase(_dbName);
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
                var table = _db.GetCollection<T>(tableName);
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
                var table = _db.GetCollection<T>(tableName);
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
                var table = _db.GetCollection<T>(tableName);
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
            throw new NotImplementedException();
        }
    }
}