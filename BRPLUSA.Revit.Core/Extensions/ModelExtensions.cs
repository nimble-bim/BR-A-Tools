using System;
using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Core.Extensions
{
    public static class ModelExtensions
    {

        public static bool HasParameter(this Document doc, Definition parameter)
        {
            var binding = doc.ParameterBindings.get_Item(parameter);

            return binding != null;
        }

        public static bool HasParameter(this Document doc, BuiltInCategory category, string parameterName)
        {
            var par = GetParameterFromCategory(doc, category, parameterName);

            return par != null;
        }

        public static Parameter GetParameterFromCategory(this Document doc, BuiltInCategory category, string name)
        {
            try
            {
                var elem = new FilteredElementCollector(doc).OfCategory(category).FirstElement();

                return elem.GetParameterFromElement(name);
            }

            catch (Exception e)
            {
                return null;
            }
        }
    }
}
