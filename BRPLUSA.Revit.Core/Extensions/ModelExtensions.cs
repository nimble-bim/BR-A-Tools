using System;
using System.Linq;
using Autodesk.Revit.DB;
using BRPLUSA.Revit.Core.Exceptions;

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
            var elem = new FilteredElementCollector(doc)
                .OfCategoryId(new ElementId(category))
                .FirstOrDefault(e => e != null);

            if(elem == null)
                throw new SpaceCreationException("Couldn't find element of this category!");

            var parameter = elem.GetParameterFromElement(name);

            return parameter;
        }
    }
}
