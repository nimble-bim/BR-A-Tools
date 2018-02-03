using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Domain.Entities
{
    public enum WorksharingEventType
    {
        Unknown = -2,
        None = -1,
        DefaultState = 0,
        ModelOpen = 1,
        ModelSyncing = 2,
        ModelSynced = 3,
        ModelClosed = 4,
        AllUsersExited = 5
    }
}
