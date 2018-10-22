namespace BRPLUSA.Domain.Services.Ventilation
{
    public abstract class BaseVentilationCalculationService
    {
        protected const double Time = 60.0;
        protected ILookupService _lookupService;

        protected BaseVentilationCalculationService(ILookupService service)
        {
            _lookupService = service;
        }

        //public abstract double CalculateMaxCFMByComparison(Space space);
    }
}
