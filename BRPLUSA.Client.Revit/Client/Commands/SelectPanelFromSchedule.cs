using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using BRPLUSA.Client.Revit.Base;
using BRPLUSA.Client.Revit.Services;

namespace BRPLUSA.Client.Revit.Client.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SelectPanelFromSchedule : BaseCommand
    {
        protected override Result Work()
        {
            try
            {
                var data = ElementPresenter.RequestPanelData(CurrentApplication);
                var element = ElementFinder.FindPanelSchedule(CurrentDocument, data.FieldName, data.FieldValue);

                if (element == null)
                    return Result.Failed;

                ElementPresenter.ShowElement(element, CurrentApplication);

                return Result.Succeeded;
            }

            catch (Exception e)
            {
                if (e.Message.Equals("Not in Panel Schedule View"))
                    TaskDialog.Show("Error", "This command can only be used from a Panel Schedule. Please open a Panel Schedule and try again.");

                return e.Message.Equals("User cancelled operation") 
                    ? Result.Cancelled 
                    : Result.Failed;
            }
        }
    }
}
