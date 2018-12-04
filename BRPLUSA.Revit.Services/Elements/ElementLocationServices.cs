using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Services.Utilities;
using ArgumentException = Autodesk.Revit.Exceptions.ArgumentException;

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

        /// <summary>
        /// TODO: make use of multiple quick filters
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindIntersectingElementsFast<T>(Element element) where T: Element
        {
            return FindIntersectingElementByPosition<T>(element);
        }

        public static IEnumerable<Element> FindIntersectingElementsSlow(Element element)
        {
            IEnumerable<Element> collected = null;

            try
            {
                var doc = element.Document;
                collected = new FilteredElementCollector(doc)
                    .OfCategoryId(element.Id)
                    .WherePasses(new ElementIntersectsElementFilter(element))
                    .ToElements();
            }

            catch (ArgumentException e)
            {
                LoggingService.LogError($"Unable to capture intersecting elements using this types of element: {element.Category.Name}",e);
                return collected;
            }

            return collected;
        }

        public static IEnumerable<T> FindIntersectingElements<T>(Element element) where T : Element
        {
            IEnumerable<T> final = null;

            try
            {
                var elems = FindIntersectingElementsSlow(element);

                final = elems.Cast<T>();
            }

            catch (Exception e)
            {
                LoggingService.LogError($"Issues casting elements of type: {typeof(T)}", e);
            }

            return final;
        }

        public static T FindIntersectingElement<T>(Element element) where T : Element
        {
            return FindIntersectingElements<T>(element).First();
        }

        public static IEnumerable<T> FindIntersectingElementByPosition<T>(Element element) where T : Element
        {
            IEnumerable<T> collected = null;

            try
            {
                var doc = element.Document;
                var box = element.get_BoundingBox(null);
                var outline = new Outline(box.Min, box.Max);
                var filter = new BoundingBoxIntersectsFilter(outline);
                collected = new FilteredElementCollector(doc).WherePasses(filter).OfType<T>();
            }

            catch (ArgumentException e)
            {
                LoggingService.LogError($"Unable to capture intersecting elements using this types of element: {element.Category.Name}", e);
                return collected;
            }

            catch (Exception e)
            {
                LoggingService.LogError($"Unable to capture any elements because of a fatal error ", e);
                return collected;
            }

            return collected;
        }

        public static Element FindIntersectingElement(Element element)
        {
            return FindIntersectingElementsSlow(element).First();
        }
    }
}
