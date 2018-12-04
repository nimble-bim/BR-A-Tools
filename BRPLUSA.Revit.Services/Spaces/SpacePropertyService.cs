using System;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Services.Elements;

namespace BRPLUSA.Revit.Services.Spaces
{
    public class SpacePropertyService
    {
        public static string GetSpaceTypeAsString(Space space)
        {
            string name = "";

            try
            {
                name = space.Document.GetElement(space.SpaceTypeId).Name;
            }
            catch (Exception e)
            {

            }

            return name;
        }

        public static double GetOutsideAirFromSpace(Space space)
        {
            double oAirPercent = 0.0;

            try
            {
                var param = space.ParametersMap.get_Item("Outside Air Percentage");

                if (param != null)
                    oAirPercent = param.AsDouble();
            }

            catch (Exception e)
            {
                Console.WriteLine($"This caused an error {e}");
            }

            return oAirPercent;
        }

        public static double CalculateCeilingHeight(Space space)
        {
            const double typCeilingHeight = 8.0;
            double height = 0;

            try
            {
                if(space?.Volume > 0.0)
                    return CalculateCeilingHeightByVolume(space);

                var roomClgHeight = CalculateCeilingHeightByCeiling(space);
                var heightByLimits = CalculateCeilingHeightByLimits(space);

                var roomHasCloserCalc = Math.Abs(roomClgHeight - roomClgHeight) <
                                        Math.Abs(typCeilingHeight - heightByLimits);

                height = roomHasCloserCalc
                    ? roomClgHeight 
                    : heightByLimits;
            }

            catch (Exception e)
            {
                LoggingService.LogError("Issue attempting to calculate ceiling height", e);
            }

            return height;
        }

        public static double CalculateCeilingHeightByCeiling(Space space)
        {
            double height = 0;

            try
            {
                var ceiling = FindCeilingInSpace(space);
                var clgBox = ceiling.get_BoundingBox(null);
                var level = (Level) space.Document.GetElement(ceiling.LevelId);
                var levelHeight = level.Elevation;

                height = clgBox.Min.Z - levelHeight;
            }

            catch (Exception e)
            {
                LoggingService.LogError("Issue attempting to calculate ceiling height based on the ceiling in the space", e);
            }

            return height;
        }

        public static double CalculateCeilingHeightByLimits(Space space)
        {
            var baseOffset = space.BaseOffset;
            var upperLimit = space.LimitOffset;
            var lowerLimit = space.Level.Elevation;

            // sometimes the height isn't calculated properly because the 
            // limit offset and base offset aren't properly calculated
            var height = upperLimit - lowerLimit;

            return height > 0 
                ? height 
                : 0;
        }

        public static double CalculateCeilingHeightByVolume(Space space)
        {
            var height = 0.0;

            try
            {
                height = space.Volume / space.Area;
            }

            catch (Exception e)
            {
                LoggingService.LogError("Can't find ceiling height by volume since Volumes are not being calculated for spaces", e);
            }

            return height;
        }

        public static Ceiling FindCeilingInSpace(Space space)
        {
            var ceilings = ElementLocationServices.FindIntersectingElementByPosition<Ceiling>(space);

            var clg = ceilings.First();

            return clg;
        }
    }
}
