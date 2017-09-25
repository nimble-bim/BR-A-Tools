using System.Collections.Generic;
using Autodesk.Revit.DB.Mechanical;

namespace BRPLUSA.Entities.Wrappers
{
    public class SpaceWrapper
    {
        public string Id { get; set; }
        public string SpaceName { get; set; }
        public string SpaceNumber { get; set; }
        public string RoomName { get; set; }
        public string RoomNumber { get; set; }
        public double SpecifiedSupplyAirflow { get; set; }
        public double SpecifiedExhaustAirflow { get; set; }
        public double SpecifiedReturnAirflow { get; set; }
        public IEnumerable<string> ConnectedSpaces { get; set; }

        public SpaceWrapper() { }

        public SpaceWrapper(Space rev)
        {
            Id = rev.UniqueId;
            SpaceName = rev.Name;
            SpaceNumber = rev.Number;
            RoomName = rev.Room?.Name;
            RoomNumber = rev.Room?.Number;
            SpecifiedSupplyAirflow = rev.DesignSupplyAirflow;
            SpecifiedExhaustAirflow = rev.DesignExhaustAirflow;
            SpecifiedReturnAirflow = rev.DesignReturnAirflow;
        }
    }
}
