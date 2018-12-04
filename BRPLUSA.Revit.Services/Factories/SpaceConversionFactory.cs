using BRPLUSA.Domain.Core.Entities.Wrappers.Revit;
using BRPLUSA.Domain.Wrappers;
using BRPLUSA.Revit.Services.Spaces;
using RevitSpace = Autodesk.Revit.DB.Mechanical.Space;

namespace BRPLUSA.Revit.Services.Factories
{
    public class SpaceConversionFactory : RevitConversionFactory<Space, RevitSpace >
    {
        public override Space Create(RevitSpace rSpace)
        {
            var cHeight = SpacePropertyService.CalculateCeilingHeight(rSpace);
            var spaceType = SpacePropertyService.GetSpaceTypeAsString(rSpace);
            var oAir = SpacePropertyService.GetOutsideAirFromSpace(rSpace);

            var space = new Space
            {
                Id = rSpace.UniqueId,
                SpaceName = rSpace.Name,
                SpaceNumber = rSpace.Number,
                RoomName = rSpace.Room?.Name,
                RoomNumber = rSpace.Room?.Number,

                Area = rSpace.Area,
                SpecifiedExhaustAirflow = rSpace.ActualExhaustAirflow,
                SpecifiedSupplyAirflow = rSpace.ActualSupplyAirflow,
                SpecifiedReturnAirflow = rSpace.ActualReturnAirflow,
                NumberOfPeople = rSpace.NumberofPeople,
                OccupancyCategory = spaceType,
                CeilingHeight = cHeight == 0.0 ? rSpace.UnboundedHeight : cHeight,
                PercentageOfOutsideAir = oAir
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
