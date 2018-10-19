using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace BRPLUSA.Client.Revit.Services
{
    public class RevitElementLocationServices
    {
        public static Element GetClosest(List<Element> likely, Element original)
        {
            Element closest = null;
            XYZ dist = new XYZ(1000, 1000, 1000);

            foreach (var e in likely)
            {
                var sub = ((LocationPoint)original.Location).Point - ((LocationPoint)e.Location).Point;

                var curr = new XYZ(Math.Abs(sub.X), Math.Abs(sub.Y), Math.Abs(sub.Z));

                if (curr.IsGreaterThanOrEqualTo(dist))
                    continue;

                dist = curr;
                closest = e;
            }

            return closest;
        }
    }

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
