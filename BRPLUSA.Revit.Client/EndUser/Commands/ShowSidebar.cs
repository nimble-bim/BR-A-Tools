using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Client.Base;
using BRPLUSA.Revit.Client.UI.Views;

namespace BRPLUSA.Revit.Client.EndUser.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ShowSidebar : BaseCommand
    {
        protected override Result Work()
        {
            var dash = CurrentApplication.GetDockablePane(BardWebClient.Id);

            if (dash.IsShown())
                dash.Hide();
            else
                dash.Show();

            return Result.Succeeded;
        }
    }
}
