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
    public class WorksharingEvent : Entity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public User User { get; set; }
        public WorksharingEventType EventType { get; set; }
        public string ModelName { get; set; }
        public string TimeCreated { get; set; }

        public WorksharingEvent()
        {
            TimeCreated = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }

        public WorksharingEvent(WorksharingEvent other)
        {
            ModelName = other.ModelName;
            User = other.User;
        }
    }

    public class DefaultWorksharingEvent : WorksharingEvent
    {
        public DefaultWorksharingEvent()
        {
            EventType = WorksharingEventType.DefaultState;
        }
    }
}
