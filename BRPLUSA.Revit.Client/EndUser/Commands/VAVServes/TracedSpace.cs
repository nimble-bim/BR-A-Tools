using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Revit.Client.EndUser.Commands.VAVServes
{
    internal class TracedSpace
    {
        public int EquipmentId { get; set; }
        public string SpaceName { get; private set; }
        public int? SpaceId { get; private set; }
        public TracedSpace(int equipmentId, string spaceName, int? spaceId)
        {
            EquipmentId = equipmentId;
            SpaceName = spaceName;
            SpaceId = spaceId;
        }
    }
}
