using System;
using BRPLUSA.Domain.Core.Wrappers;
using BRPLUSA.Domain.Wrappers;

namespace BRPLUSA.Domain.Services.Ventilation
{
    public class VentilationCalculationService
    {
        public static double CalculateCFMBasedOnSupplyACH(Space space)
        {
            return CalculationService_ASHRAE_170.CalculateCFMBasedOnSupplyACH(
                space.Area, 
                space.CeilingHeight, 
                space.OccupancyCategory);
        }

        public static double CalculateCFMBasedOnVentACH(Space space)
        {
            return CalculationService_ASHRAE_170.CalculateCFMBasedOnVentACH(
                space.Area, 
                space.CeilingHeight, 
                space.PercentageOfOutsideAir, 
                space.OccupancyCategory);
        }

        public static double CalculateModeledPressurization(Space space)
        {
            var negativePressure = Math.Abs(space.SpecifiedExhaustAirflow) + Math.Abs(space.SpecifiedReturnAirflow);
            var pressure = space.SpecifiedSupplyAirflow - negativePressure;

            return pressure;
        }
    }
}
