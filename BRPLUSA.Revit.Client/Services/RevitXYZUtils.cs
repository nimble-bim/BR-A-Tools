using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Client.Services
{
    public static class RevitXYZUtils
    {
        public static bool IsLessThan(this XYZ one, XYZ two)
        {
            bool result = (one.X < two.X) && (one.Y < two.Y) && (one.Z < two.Z);

            return result;
        }

        public static bool IsGreaterThan(this XYZ xyz1, XYZ xyz2)
        {
            return !xyz1.IsLessThan(xyz2);
        }

        public static bool IsLessThanOrEqualTo(this XYZ one, XYZ two)
        {
            bool result = (one.X <= two.X) && (one.Y <= two.Y) && (one.Z <= two.Z);

            return result;
        }

        public static bool IsGreaterThanOrEqualTo(this XYZ one, XYZ two)
        {
            return !one.IsLessThanOrEqualTo(two);
        }
    }
}