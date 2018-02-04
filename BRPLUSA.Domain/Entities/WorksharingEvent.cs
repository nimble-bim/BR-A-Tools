using BRPLUSA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BRPLUSA.Domain.Entities
{
    public abstract class WorksharingEvent : Entity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public User User { get; set; }
        public WorksharingEventType EventType { get; set; }
        public string ModelName { get; set; }
        public string TimeCreated { get; set; }

        protected WorksharingEvent()
        {
            TimeCreated = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Initialize();
        }

        protected WorksharingEvent(WorksharingEvent other) : this()
        {
            ModelName = other.ModelName;
            User = other.User;
        }

        protected WorksharingEvent(User user) : this()
        {
            User = user;
        }

        protected WorksharingEvent(string modelName) : this()
        {
            ModelName = modelName;
            User = new CurrentUser();
        }

        protected abstract void Initialize();
    }

    public class DefaultWorksharingEvent : WorksharingEvent
    {

        //public DefaultWorksharingEvent()
        //{
            
        //}
        protected override void Initialize()
        {
            EventType = WorksharingEventType.DefaultState;
        }
    }
}
