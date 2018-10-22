using System;

namespace BRPLUSA.Domain.Services.Ventilation
{
    public class CalculationService_2014MC : BaseVentilationCalculationService
    {
        public CalculationService_2014MC(ILookupService service) : base(service) { }

        public double CalculateExhaustCFMByArea(Space space)
        {
            // Lookup table Exhaust Reqs * Space.Area

            throw new NotImplementedException();

        }

        public double CalculateExhaustCFMByFixtureCount(Space space)
        {
            // Lookup table Exhaust Reqs * Space.#OfFixtures

            throw new NotImplementedException();
        }

        //public override double CalculateMaxCFMByComparison(Space space)
        //{
        //    throw new NotImplementedException();
        //}

        /*
         * Need a few more things
         *
         * CFMByArea - Does the current Space Exhaust CFM meet requirements
         *
         */
    }
}