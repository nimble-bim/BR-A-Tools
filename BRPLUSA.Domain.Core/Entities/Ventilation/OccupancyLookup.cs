namespace BRPLUSA.Domain.Entities.Ventilation
{
    public class OccupancyLookup
    {
        public string OccupancyCategory { get; set; }
        public MechanicalCode_2013 MechCode2013 { get; set; }
        public MechanicalCode_2014 MechCode2014 { get; set; }
        public MechanicalCode_ASHRAE_170 MechCodeAshrae { get; set; }
        public LightingEquipmentLoad LightingEquipmentLoad { get; set; }
    }
}