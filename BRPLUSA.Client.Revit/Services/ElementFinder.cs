using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BRPLUSA.Core.Extensions;

namespace BRPLUSA.Revit.Services
{
    public static class ElementFinder
    {
        private static ParameterValueProvider FindNamedParameter(string field)
        {
            throw new NotImplementedException();
        }

        public static Element FindTextInView(Document doc, View view, string searchValue)
        {
            var filter = CreateTextFilter(view);

            var textItems = new FilteredElementCollector(doc, view.Id).WherePasses(filter).ToElements();

            return FilterText(textItems, searchValue);
        }

        public static Element FindElementByName(Document doc, string value, string field = "mark")
        {
            //return field == "mark" 
            //    ? FindByMark(doc, value) 
            //    : SearchElementParametersByHash(doc, field, value).Element;

            return field == "mark"
                ? FindByMark(doc, value)
                : SearchElementSimply(doc, field, value).Element;
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

        /// <summary>
        /// Runtime is about O(n) * O(1): O(n)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        private static Parameter SearchElementParametersByHash(Document doc, string fieldName, string fieldValue)
        {
            try
            {
                //var iter = new FilteredElementCollector(doc)
                //    .WhereElementIsNotElementType()
                //    .GetElementIterator();

                var iter = new FilteredElementCollector(doc)
                    .WhereElementIsViewIndependent()
                    .ToElements().ToArray();

                Parameter possible = null;

                Parallel.ForEach(iter, (elem) =>
                {
                    // skip any elements that don't have the parameter
                    var temp = TryGetParameter(elem, fieldName);

                    var val = possible?.AsValueString();

                    if (val == fieldValue)
                        possible = temp;
                });

                // store parameter name, paramter value and elementId
                //while (iter.MoveNext())
                //{
                //    var elem = iter.Current;

                //    // skip any elements that don't have the parameter
                //    var possible = TryGetParameter(elem, fieldName);

                //    if(possible == null)
                //        continue;

                //    var val = possible.AsValueString();

                //    if (val == fieldValue)
                //        return possible;
                //}

                if (possible != null)
                    return possible;

                TaskDialog.Show("Doesn't exist", "The requested parameter does not exist in the model");
                throw new Exception("Could not find parameter");
            }

            catch (Exception e)
            {
                throw new Exception("Could not find parameter", e);
            }
        }

        private static Parameter TryGetParameter(Element elem, string fieldName)
        {
            try
            {
                var possible = elem?.ParametersMap?.get_Item(fieldName);

                return possible;
            }

            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Runtime is about O(n) * O(n): O(n^2)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        private static Parameter SearchElementSimply(Document doc, string fieldName, string fieldValue)
        {
            try
            {
                var iter = new FilteredElementCollector(doc)
                    .WhereElementIsElementType()
                    .ToElements();

                var i = 0;

                // store parameter name, paramter value and elementId
                foreach(var elem in iter)
                {
                    i++;

                    if (elem == null)
                        continue;

                    foreach (var p in elem.GetOrderedParameters())
                    {
                        if (p?.Definition?.Name == null)
                            continue;

                        if (String.Equals(p?.Definition?.Name, fieldName, StringComparison.CurrentCultureIgnoreCase) &&
                            String.Equals(p.AsValueString(), fieldValue, StringComparison.CurrentCultureIgnoreCase))
                            return p;
                    }
                }

                TaskDialog.Show("Doesn't exist", "The requested parameter does not exist in the model");
                throw new Exception("Could not find parameter");
            }

            catch (Exception e)
            {
                throw new Exception("Could not find parameter", e);
            }
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
            
            // remove nulls
            var array = store.ToArray().Where(p => p?.Definition?.Name != null).ToArray();

            // quicksort the parameters
            var parameters = QuickSortParameters(array);

            // then binary search it for the parameter in question
            var parameter = FindParameter(parameters, fieldName, fieldValue);

            // return the parameter
            return parameter;
        }

        private static Parameter FindParameter(Parameter[] array, string paramName, string paramValue)
        {
            var low = 0;
            var high = array.Length - 1;

            while (low <= high)
            {
                var mid = (low + high) / 2;

                var guess = array[mid];
                var searchField = paramName + paramValue;

                var current = guess.Definition.Name + guess.AsValueString();

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
            var lesser = new List<Parameter>();
            var greater = new List<Parameter>();

            foreach (var p in pars)
            {
                if(pivot.IsBeforeInOrder(p))
                    lesser.Add(p);
                else
                {
                    greater.Add(p);
                }
            }

            var list = new List<Parameter>();

            var less = QuickSortParameters(lesser.ToArray());
            var great = QuickSortParameters(greater.ToArray());

            list.AddRange(less);
            list.Add(pivot);
            list.AddRange(great);

            return list.ToArray();
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

        public static Element FilterText(ICollection<Element> items, string textValue)
        {
            var text = items.Cast<TextNote>();
            var possible = text.Where(n => n.Text.Equals(textValue)).ToArray();

            if (possible.Length < 1)
                return HandleElementNotFound();

            if (possible.Length == 1)
                return HandleFoundElement(items.First());

            TaskDialog.Show("Multiple Elements",
                "There are multiple elements matching the criteria provided - we'll present them to you with an option for selection");

            return HandleFoundElements(new Stack<Element>(possible));
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

        private static ElementFilter CreateTextFilter(View view)
        {
            var filter = new ElementOwnerViewFilter(view.Id);

            return filter;
        }

        private static bool IsBeforeInOrder(this Parameter pivot, Parameter p)
        {
            try
            {
                if (p?.Definition?.Name == null)
                    return true;

                var pivotP = (pivot?.Definition?.Name + pivot?.AsValueString()).ToLower();
                var checkP = (p?.Definition?.Name + p?.AsValueString()).ToLower();

                var isBefore = pivotP.IsLessThan(checkP);

                return isBefore;
            }

            catch (Exception e)
            {
                throw new Exception("The parameter could no be processed", e);
            }
        }

        private static bool IsAfterInOrder(this Parameter pivot, Parameter p)
        {
            return !pivot.IsBeforeInOrder(p);
        }

    }
}
