using BRPLUSA.Domain.Wrappers;
using BRPLUSA.Revit.Services.Spaces;
using RevitSpace = Autodesk.Revit.DB.Mechanical.Space;

namespace BRPLUSA.Revit.Services.Factories
{
    public class SpaceConversionFactory : RevitConversionFactory<Space, RevitSpace >
    {
        public override Space Create(RevitSpace rSpace)
        {
            var space = new Space
            {
                Area = rSpace.Area,
                SpecifiedExhaustAirflow = rSpace.ActualExhaustAirflow,
                SpecifiedSupplyAirflow = rSpace.ActualSupplyAirflow,
                SpecifiedReturnAirflow = rSpace.ActualReturnAirflow,
                NumberOfPeople = rSpace.NumberofPeople,
                OccupancyCategory = SpacePropertyService.GetSpaceTypeAsString(rSpace),
                CeilingHeight = SpacePropertyService.CalculateCeilingHeight(rSpace),
                PercentageOfOutsideAir = SpacePropertyService.GetOutsideAirFromSpace(rSpace)
            };

            return space;
        }

        public override Space Create()
        {
            return new Space();
        }

        public override Space Create(object obj)
        {
            return Create();
        }
    }
}
