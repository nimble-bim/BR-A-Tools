using System;
using System.Collections.Generic;
using Autodesk.Revit.DB.Mechanical;

namespace BRPLUSA.Client.Revit.Entities.Wrappers
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
        public IEnumerable<string> ConnectedSpaces { get; protected set; }

        public void ConnectPeers(IEnumerable<string> spaces)
        {
            ConnectedSpaces = spaces;
        }

        public bool ExhaustNeedsUpdate(Space space)
        {
            return Math.Abs(space.DesignExhaustAirflow - SpecifiedExhaustAirflow) > .0001;
        }

        public bool ReturnNeedsUpate(Space space)
        {
            return Math.Abs(space.DesignReturnAirflow - SpecifiedReturnAirflow) > .0001;
        }

        public bool SupplyNeedsUpdate(Space space)
        {
            return Math.Abs(space.DesignSupplyAirflow - SpecifiedSupplyAirflow) > .0001;
        }

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
