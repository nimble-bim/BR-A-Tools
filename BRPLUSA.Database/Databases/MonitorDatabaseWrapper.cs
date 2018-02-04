using BRPLUSA.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Database.Base;

namespace BRPLUSA.Database.Databases
{
    public class WorksharingMonitorDb : BaseDatabaseWrapper<WorksharingEvent>
    {
        

    }

    public class WorksharingMonitorTable
    {
        private readonly string _tableName; 
        public BaseDatabaseWrapper<WorksharingEvent> Database { get; private set; }
        public IMongoCollection<WorksharingEvent> Table { get; private set; }
        public WorksharingEvent CurrentState { get; private set; }

        public WorksharingMonitorTable(string modelName)
        {
            _tableName = modelName;
            Initialize();
        }

        private void Initialize()
        {
            Database = new WorksharingMonitorDb();
            Table = Database.CreateTableIfNotExists(_tableName);
            CurrentState = ConfirmDefaultState();
        }

        private WorksharingEvent ConfirmDefaultState()
        {
            // apparently _ => true does the same thing
            //var all = new FilterDefinitionBuilder<WorksharingEvent>().Empty;
            var docCount = Table.Count(_ => true);
            var hasElements = docCount > 0;

            return !hasElements
                ? CreateDefaultState() 
                : GetLastInserted();
        }

        public WorksharingEvent GetLastInserted()
        {
            var all = new FilterDefinitionBuilder<WorksharingEvent>().Empty;
            var lastItem = Table
                            .Find(all)
                            .Sort(
                                new SortDefinitionBuilder<WorksharingEvent>()
                                .Descending("$natural"))
                                .Limit(1).ToList().ToArray();

            var lastInserted = Table.Find(all)
                                .Sort(new SortDefinitionBuilder<WorksharingEvent>()
                                .Descending(ws => ws.TimeCreated))
                                .Limit(1).ToList().ToArray();

            //return lastItem[lastItem.Length - 1];
            return lastInserted[lastInserted.Length - 1];
        }

        private WorksharingEvent CreateDefaultState()
        {
            var state = new DefaultWorksharingEvent();
            Table.InsertOne(state);
            return state;
        }

        public async void AddModelOpenedEvent(string modelName)
        {
            await Table.InsertOneAsync(new UserOpenedModelEvent(modelName));
        }
    }
}
