using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Base;
using BRPLUSA.Revit.Services;

namespace BRPLUSA.Revit.Client.Commands
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
