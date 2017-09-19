using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA_Tools
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
    }
}
