namespace BRPLUSA.Domain.Entities.Ventilation
{
    public class MechanicalCode_ASHRAE_170
    {
        public double? VentilationAirChangesPerHour { get; set; }
        public double? SupplyAirChangesPerHour { get; set; }
        public PressureRelationship PressureRelationship { get; set; }
        public bool? AllRoomAirExhausted { get; set; }
    }
}