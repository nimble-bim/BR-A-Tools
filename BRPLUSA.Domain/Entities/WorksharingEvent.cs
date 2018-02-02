using BRPLUSA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Domain.Entities
{
    public class WorksharingEvent : Entity
    {
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
