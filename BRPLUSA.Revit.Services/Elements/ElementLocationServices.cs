using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using BRPLUSA.Revit.Services.Utilities;

namespace BRPLUSA.Revit.Services.Elements
{
    public class ElementLocationServices
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

        public static IEnumerable<Element> FindElementsInSameSpace(Element element)
        {
            var doc = element.Document;
            var collected = new FilteredElementCollector(doc)
                .OfCategoryId(element.Id)
                .WherePasses(new ElementIntersectsElementFilter(element))
                .ToElements();

            return collected;
        }

        public static IEnumerable<T> FindElementsInSameSpace<T>(Element element) where T : Element
        {
            var elem = FindElementsInSameSpace(element).Cast<T>();

            return elem;
        }

        public static T FindElementInSameSpace<T>(Element element) where T : Element
        {
            return FindElementsInSameSpace<T>(element).First();
        }

        public static Element FindElementInSameSpace(Element element)
        {
            return FindElementsInSameSpace(element).First();
        }
    }
}
