using System;
using BRPLUSA.Domain.Core.Entities.Wrappers.Revit;
using BRPLUSA.Domain.Wrappers;

namespace BRPLUSA.Domain.Services.Ventilation
{
    public class CalculationService_ASHRAE_621 : BaseVentilationCalculationService
    {
        public CalculationService_ASHRAE_621(ILookupService service) : base(service) { }

        public double CalculateCFMBasedOnExhaustACH()
        {
            throw new NotImplementedException();
        }

        public double CalculateCFMBasedOnVentACH(Space space)
        {
            throw new NotImplementedException();
        }

        //public override double CalculateMaxCFMByComparison(Space space)
        //{
        //    throw new NotImplementedException();
        //}
    }
}