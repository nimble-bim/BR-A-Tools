using BRPLUSA.Domain.Base;
using System;
using System.Collections.Generic;
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
        public string[] UsersSyncing { get; set; }
        public string[] UsersCurrentlyInModel { get; set; }
        public WorksharingEventType EventType { get; set; }
        public string ModelName { get; set; }
        public string TimeCreated { get; set; }

        public WorksharingEvent()
        {
            TimeCreated = DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString();
        }

        public WorksharingEvent(WorksharingEvent other)
        {
            UsersSyncing = other.UsersSyncing;
            UsersCurrentlyInModel = other.UsersCurrentlyInModel;
            ModelName = other.ModelName;
        }
    }
}
