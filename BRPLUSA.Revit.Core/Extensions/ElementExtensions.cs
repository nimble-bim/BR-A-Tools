using System;
using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Core.Extensions
{
    public static class ElementExtensions
    {
        public static bool HasParameter(this Element element, string parameterName)
        {
            return GetParameterFromElement(element, parameterName) != null;
        }

        public static Parameter GetParameterFromElement(this Element element, string parameterName)
        {
            try
            {
                var parameter = element.ParametersMap.get_Item(parameterName);

                if (parameter == null)
                    throw new Exception("Parameter was not found in the element");

                return parameter;
            }

            catch (Exception e)
            {
                return null;
            }
        }

        public static void SetParameterValue(this Element elem, string name, string val)
        {
            var doc = elem.Document;

            using (var tr = new Transaction(doc))
            {
                if (!tr.HasStarted())
                    tr.Start("Setting parameter value");

                try
                {
                    var par = elem.ParametersMap.get_Item(name);

                    SetParameterBasedOnType(par, val);

                    tr.Commit();
                }

                catch (Exception e)
                {
                    tr.RollBack();
                }
            }
        }

        private static void SetParameterBasedOnType(this Parameter par, string val)
        {
            var kind = par.StorageType;

            switch (kind)
            {
                case StorageType.Double:
                    var pVal = double.Parse(val);
                    par.Set(pVal);
                    break;
                case StorageType.Integer:
                    par.Set(int.Parse(val));
                    break;
                default:
                case StorageType.None:
                case StorageType.String:
                    par.Set(val);
                    break;
            }
        }
    }
}
