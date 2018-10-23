using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BRPLUSA.Revit.Client.Base;
using BRPLUSA.Revit.Services.Elements;

namespace BRPLUSA.Revit.Client.EndUser.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ForceTypeSwap : BaseCommand
    {
        protected override Result Work()
        {
            var result = SwapType();

            return result ? Result.Succeeded : Result.Failed;
        }

        protected bool SwapType()
        {
            // select element
            var elem = SelectElement();

            // find closest element
            var linked = FindMonitoredElementInSameSpace(elem);

            // replace that element
            var replaced = ReplaceElementType(elem, linked);

            return replaced;
        }

        protected Element SelectElement()
        {
            Element local;
            bool result;

            using(var sel = UiDocument.Selection)
            {
                var refer = sel.PickObject(ObjectType.Element, "Select the item you'd like to update");
                local = CurrentDocument.GetElement(refer);
            }

            if (local == null || !local.IsMonitoringLinkElement())
                return null;

            return local;
        }

        protected Element FindMonitoredElementInSameSpace(Element local)
        {
            var id = local.GetMonitoredLinkElementIds().First();

            var linkInst = (RevitLinkInstance)CurrentDocument.GetElement(id);
            var linkDoc = linkInst.GetLinkDocument();

            var elems = CaptureElementsOfSameCategory(local, linkDoc);

            var final = FilterElements(elems.ToArray(), (FamilyInstance)local);

            return final;
        }

        protected IEnumerable<Element> CaptureElementsOfCategory(Element elem, Document doc, Category cat)
        {
            var collected = new FilteredElementCollector(doc)
                .OfCategoryId(cat.Id)
                .WherePasses(new ElementIntersectsElementFilter(elem))
                .ToElements();

            return collected;
        }

        protected IEnumerable<Element> CaptureElementsOfSameCategory(Element elem, Document doc)
        {
            return CaptureElementsOfCategory(elem, doc, elem.Category);
        }

        protected Element FilterElements(Element[] elements, FamilyInstance orig)
        {
            var insts = elements.Where(e => e is FamilyInstance)
                            .Cast<FamilyInstance>()
                            .ToArray();

            if (insts.Length == 1)
                return insts.First();

            var likely = new List<Element>();

            foreach (var e in insts)
            {
                if (e.Symbol.Name != orig.Symbol.Name)
                    continue;

                var eLoc = ((LocationPoint)e.Location).Point;
                var oLoc = ((LocationPoint)orig.Location).Point;

                if (eLoc == oLoc)
                    return e;

                likely.Add(e);
            }

            return ElementLocationServices.GetClosest(likely, orig);
        }

        public bool ReplaceElementType(Element oElem, Element nElem)
        {
            // find the type of that element in the current project
            var elemType = FindElementTypeInModel(nElem);

            if(elemType != null)
                return false;

            var oInst = (FamilyInstance)oElem;
            var nInst = (FamilyInstance)nElem;

            using (var tr = new Transaction(CurrentDocument))
            {
                oInst.Symbol = nInst.Symbol;

                tr.Commit();
            }

            return true;
        }

        protected FamilySymbol FindElementTypeInModel(Element elem)
        {
            var inst = (FamilyInstance)elem;
            var symbol = inst.Symbol;

            var linkedSymbol = new FilteredElementCollector(CurrentDocument)
                                .WherePasses(new FamilySymbolFilter(symbol.Family.Id))
                                .Where(sym => sym.Name == symbol.Name)
                                .Cast<FamilySymbol>()
                                .Single();

            return linkedSymbol;
        }
    }
}
