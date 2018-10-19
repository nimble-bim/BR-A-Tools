using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using BRPLUSA.Revit.Services.Utilities;

namespace BRPLUSA.Revit.Services.Elements
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
}
