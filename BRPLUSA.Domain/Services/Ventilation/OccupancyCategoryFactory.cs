using System;

namespace BRPLUSA.Domain.Services.Ventilation
{
    public class OccupancyCategoryFactory
    {
        public static OccupancyLookup Create(string[] row)
        {
            var occCat = row[0];
            var iprp = GetDoubleFromString(row[1]);
            var ipra = GetDoubleFromString(row[2]);
            var sirp = GetDoubleFromString(row[3]);
            var sira = GetDoubleFromString(row[4]);
            var defaultDens = GetDoubleFromString(row[5]);
            var exhReqs2013 = GetDoubleFromString(row[6]);
            //var blank0 = row[7);

            // 2014 mech code reqs
            var outdoorAir = GetDoubleFromString(row[8]);
            var maxOccDen = GetDoubleFromString(row[9]);
            var exhReqs2014 = GetDoubleFromString(row[10]);
            //var blank0 = row[11)

            // ashrae 170
            var ventACPH = GetDoubleFromString(row[12]);
            var supplyACPH = GetDoubleFromString(row[13]);
            var pressure = row[14];
            var rmExh = row[15] == "YES";

            var pEnum = pressure == "NR"
                ? PressureRelationship.None
                : (pressure == "+"
                    ? PressureRelationship.Positive
                    : PressureRelationship.Negative);

            //var blank0 = row[16)
            // lighting & eq table
            var eqLoad = GetDoubleFromString(row[17]);
            var ltLoad = GetDoubleFromString(row[18]);

            var thing = new OccupancyLookup
            {
                OccupancyCategory = occCat,
                MechCode2013 = new MechanicalCode_2013
                {
                    IP = new IP
                    {
                        Rp = iprp,
                        Ra = ipra
                    },
                    SI = new SI
                    {
                        Rp = sirp,
                        Ra = sira,
                    },
                    DefaultOccupancyDensity = defaultDens,
                    ExhaustRequirements = exhReqs2013
                },

                MechCode2014 = new MechanicalCode_2014
                {
                    OutdoorAir = outdoorAir,
                    MaxOccupancyDensity = maxOccDen,
                    ExhaustRequirement = exhReqs2014
                },

                MechCodeAshrae = new MechanicalCode_ASHRAE_170
                {
                    VentilationAirChangesPerHour = ventACPH,
                    SupplyAirChangesPerHour = supplyACPH,
                    PressureRelationship = pEnum,
                    AllRoomAirExhausted = rmExh
                },

                LightingEquipmentLoad = new LightingEquipmentLoad
                {
                    EquipmentLoad = eqLoad,
                    LightingLoad = ltLoad
                }
            };

            Console.WriteLine(thing);

            return thing;
        }

        public static double? GetDoubleFromString(string val)
        {
            double? finalVal = null;

            try
            {
                double.TryParse(val, out double conv);
                finalVal = conv;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return finalVal;
        }
    }
}
