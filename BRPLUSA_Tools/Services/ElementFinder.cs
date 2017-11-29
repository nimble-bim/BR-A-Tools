using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BRPLUSA.Revit.Services
{
    public static class ElementFinder
    {
        private static ParameterValueProvider FindNamedParameter(string field)
        {
            throw new NotImplementedException();
        }

        private static Parameter SearchElementSlow(Document doc, string fieldName, string fieldValue)
        {
            var store = new List<Parameter>();
            var iter = new FilteredElementCollector(doc)
                .WhereElementIsViewIndependent()
                .GetElementIterator();

            // store parameter name, paramter value and elementId
            while (iter.MoveNext())
            {
                var elem = iter.Current;

                if (elem == null)
                    continue;

                store.AddRange(elem.GetOrderedParameters());
            }
            
            // quicksort the parameters
            var parameters = QuickSortParameters(store.ToArray());

            // then binary search it for the parameter in question
            var parameter = FindParameter(parameters, fieldName, fieldValue);

            // return the parameter
            return parameter;
        }

        private static Parameter FindParameter(Parameter[] array, string paramName, string paramValue)
        {
            var low = 0;
            var high = array.Length - 1;
            var mid = (low + high) / 2;

            var guess = array[mid];
            var searchField = paramName + paramValue;

            var current = guess.Definition.Name + guess.AsValueString();

            while (low <= high)
            {
                if (current == searchField)
                    return guess;

                var isLesser = string.CompareOrdinal(current, searchField) < 0;

                if (isLesser)
                    low = mid + 1;
                else
                    high = mid - 1;
            }

            return null;
        }

        private static Parameter[] QuickSortParameters(Parameter[] pars)
        {
            var count = pars.Length;

            if (count < 2)
                return pars;

            var pivot = pars[count / 2];

            var lesser = pars.Where(p => string.CompareOrdinal(p?.Definition?.Name.ToLower(), pivot?.Definition?.Name.ToLower()) < 0).ToArray();
            var greater = pars.Where(p => string.CompareOrdinal(p?.Definition?.Name.ToLower(), pivot?.Definition?.Name.ToLower()) > 0).ToArray();

            var list = new List<Parameter>();

            list.AddRange(QuickSortParameters(lesser));
            list.Add(pivot);
            list.AddRange(QuickSortParameters(greater));

            return list.ToArray();
        }

        public static Element FindElementByName(Document doc, string value, string field = "mark")
        {
            return field == "mark" 
                ? FindByMark(doc, value) 
                : SearchElementSlow(doc, field, value).Element;
        }

        public static Element FindByMark(Document doc, string value)
        {
            var filter = CreateFilter("mark");

            // query the document for an element with a value like name
            var items = new FilteredElementCollector(doc)
                .WherePasses(filter)
                .ToElements();

            return FilterElements(items, value);
        }

        public static Element FindPanelSchedule(Document doc, string fieldName, string value)
        {
            var rule = ParameterFilterRuleFactory.CreateContainsRule(new ElementId(BuiltInParameter.PANEL_SCHEDULE_NAME), value, false);
            var filter = new ElementParameterFilter(rule);


            var items = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .ToElements()
                .Where(e => e.Name.ToLower().Equals(value.ToLower()))
                .ToArray();

            //foreach (var e in items)
            //{
            //    var name = e.LookupParameter("Panel Schedule Name");
            //    var panelNameParameter = e.GetOrderedParameters().FirstOrDefault(p => p.Definition.Name.Equals("Panel Schedule Name"));
            //}

            //var items = new FilteredElementCollector(doc)
            //    .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
            //    .WherePasses(filter)
            //    .ToElements();

            return FilterElements(items, value, fieldName);
        }

        private static Element FilterElements(ICollection<Element> items, string value, string fieldName = "mark")
        {
            if (items.Count < 1)
                return HandleElementNotFound();

            if (items.Count == 1)
                return HandleFoundElement(items.First());

            TaskDialog.Show("Multiple Elements",
                "There are multiple elements matching the criteria provided - we'll present them to you with an option for selection");

            return HandleFoundElements(new Stack<Element>(items));
        }

        private static Element HandleFoundElements(Stack<Element> elements)
        {
            while (elements.Count > 0)
            {
                var element = elements.Pop();

                var wanted = PresentElementForSelection(element);

                if (wanted)
                    return HandleFoundElement(element);
            }

            return HandleElementNotFound();
        }

        private static Element HandleElementNotFound()
        {
            TaskDialog.Show("Not Found", "The requested element could not be found");
            return null;
        }

        private static bool PresentElementForSelection(Element element)
        {
            var doc = element.Document;
            var level = doc.GetElement(element.LevelId);
            var result = TaskDialog.Show(
                "Multiple Elements",
                $"The element is on '{level.Name}' & ",
                TaskDialogCommonButtons.Yes,
                TaskDialogResult.No);

            return result == TaskDialogResult.Yes;
        }

        private static Element HandleFoundElement(Element element)
        {
            TaskDialog.Show("Found Element", "Selecting the requested Element");

            return element;
        }

        private static ElementFilter CreateFilter(string field)
        {
            var paramValue = field.Equals("mark")
                ? FindNamedParameter(field)
                : new ParameterValueProvider(new ElementId(BuiltInParameter.ALL_MODEL_MARK));

            FilterRule filterRule = new FilterStringRule(paramValue, new FilterStringContains(), field, false);

            ElementFilter filter = new ElementParameterFilter(filterRule);

            return filter;
        }

    }
}
