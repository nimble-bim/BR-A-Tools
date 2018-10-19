using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using BRPLUSA.Client.Revit.Base;
using BRPLUSA.Client.Revit.Services;

namespace BRPLUSA.Client.Revit.Client.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SelectByName : BaseCommand
    {
        protected override Result Work()
        {
            try
            {
                var data = ElementPresenter.RequestElementData();
                var element = ElementFinder.FindElementByName(CurrentDocument, data.FieldValue, data.FieldName);

                if (element == null)
                    return Result.Failed;

                ElementPresenter.ShowElement(element, CurrentApplication);

                return Result.Succeeded;
            }

            catch (Exception e)
            {
                return Result.Failed;
            }
        }
    }
}
