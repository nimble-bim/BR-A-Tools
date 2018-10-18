using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Revit.Client.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CreateScheduleFromExcel : BaseCommand
    {
        private ViewDrafting _draftingView;

        protected override Result Work()
        {
            // create new drafting view
            _draftingView = CreateNewDraftingView();

            // get data from excel and each of the cells
            //GetExcelData

            // get columns & row sizes

            // modify drafting view to add columns and rows
            




            return Result.Succeeded;
        }

        private ViewDrafting CreateNewDraftingView()
        {
            var drfViews = new FilteredElementCollector(CurrentDocument)
                            .OfClass(typeof(ViewFamilyType));

            var dvId = drfViews
                    .Cast<ViewFamilyType>()
                    .FirstOrDefault(v => v.ViewFamily == ViewFamily.Drafting).Id;

            var newView = ViewDrafting.Create(CurrentDocument, dvId);

            return newView;
        }

        private void GetExcelData()
        {
            var dlg = new FileOpenDialog("xlsx");
            var result = dlg.Show();
        }
    }
}
