using BRPLUSA.Domain.Entities;
using BRPLUSA.Revit.Client.Updaters;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Database.Databases
{
    public class WMTableWrapper : BaseDatabaseWrapper<WorksharingEvent>
    {
        private WorksharingEvent _currentState = null;
        private IMongoCollection<WorksharingEvent> _table;

        public WMTableWrapper(string modelName) : base()
        {
            _table = _db.GetCollection<WorksharingEvent>(modelName);

        }

        //public Task<bool> AddModelOpenedEvent(User user, string modelName)
        //{

        //}
    }
}
