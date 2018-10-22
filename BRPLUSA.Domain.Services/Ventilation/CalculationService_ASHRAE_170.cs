namespace BRPLUSA.Domain.Services.Ventilation
{
    public class CalculationService_ASHRAE_170 : BaseVentilationCalculationService
    {
        public CalculationService_ASHRAE_170(ILookupService service) : base(service) { }

        public static double CalculateCFMBasedOnVentACH(double area, double ceilingHeight, double percentageOutsideAir, string category)
        {
            // find ventACH based on lookup
            //double ventACH = _lookupService.GetOAACHRBasedOnOccupancyCategory(category);
            double ventACH = VentilationLookupService.GetOAACHRBasedOnOccupancyCategory(category);

            var temp = (ventACH * area * ceilingHeight) / Time;

            double finalCFM = temp / percentageOutsideAir;

            return finalCFM; 
        }

        public static double CalculateCFMBasedOnSupplyACH(double area, double ceilingHeight, string category)
        {
            var supplyACH = VentilationLookupService.GetACHRBasedOnOccupancyCategory(category);

            var cfm = (supplyACH * area * ceilingHeight) / Time;

            return cfm;
        }

        // Next, take the max between the previous two
        //public override double CalculateMaxCFMByComparison(Space space)
        //{
        //    var ventCFM = CalculateCFMBasedOnVentACH(space);
        //    var supplyCFM = CalculateCFMBasedOnSupplyACH(space);

        //    return ventCFM > supplyCFM ? ventCFM : supplyCFM;
        //}
    }
}