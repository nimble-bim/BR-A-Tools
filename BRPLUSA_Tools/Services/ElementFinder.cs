using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BRPLUSA.Revit.Services
{
    public static class ElementFinder
    {
        public static Element FindElementByName(Document doc, string name, string field = "mark")
        {
            var filter = CreateFilter(field);

            // query the document for an element with a value like name
            var items = new FilteredElementCollector(doc)
                            .WherePasses(filter)
                            .ToElements();

            return FilterElements(items, name, field);
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

        public static Element FindPanelSchedule(Document doc, string fieldName, string value)
        {
            var provider = new ParameterValueProvider(new ElementId(BuiltInParameter.PANEL_SCHEDULE_NAME));
            var rule = new FilterStringRule(provider, new FilterStringContains(), value, false);
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

        private static ParameterValueProvider FindNamedParameter(string field)
        {
            return null;
            //return new ParameterValueProvider();
        }
    }
}
