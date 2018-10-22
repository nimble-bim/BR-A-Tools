using BRPLUSA.Domain.Base;

namespace BRPLUSA.Domain.Wrappers
{
    public class Space : Entity
    {
        public string Id { get; set; }
        public string SpaceName { get; set; }
        public string SpaceNumber { get; set; }
        public string RoomName { get; set; }
        public string RoomNumber { get; set; }
        public double Area { get; set; }
        public double CeilingHeight { get; set; }
        public double NumberOfPeople { get; set; }
        public double PercentageOfOutsideAir { get; set; }
        public string OccupancyCategory { get; set; }
        public double SpecifiedSupplyAirflow { get; set; }
        public double SpecifiedExhaustAirflow { get; set; }
        public double SpecifiedReturnAirflow { get; set; }
    }
}
