using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace BRPLUSA.Client.Revit.Base
{

    public class RevitSelectionFilter<T> : ISelectionFilter where T : Element
    {
        public bool AllowElement(Element elem)
        {
            return elem is T;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
