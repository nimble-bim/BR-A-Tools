using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Client.Base;
using BRPLUSA.Revit.Client.UI.Services;
using BRPLUSA.Revit.Services.Elements;

namespace BRPLUSA.Revit.Client.EndUser.Commands
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
