using System;
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BRPLUSA.Base
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public abstract class BaseCommand : IExternalCommand
    {
        protected ExternalCommandData ExternalCommandData { get; set; }
        protected string MainMessage { get; set; }
        protected ElementSet ElementSet { get; set; }
        protected UIApplication CurrentApplication => ExternalCommandData.Application;
        protected Document CurrentDocument => CurrentApplication.ActiveUIDocument.Document;
        protected UIDocument UiDocument => CurrentApplication.ActiveUIDocument;

        // necessary method that Revit needs to call
        Result IExternalCommand.Execute(
            ExternalCommandData excmd,
            ref string mainmessage,
            ElementSet elemset)
        {
            MainMessage = mainmessage;
            ExternalCommandData = excmd;
            ElementSet = elemset;

            using (var tr = new Transaction(CurrentDocument))
            {
                tr.Start("Regenerating...");

                Result result;

                try
                {
                    result = InternalExecute();
                }

                catch (Exception e)
                {
                    result = Result.Failed;
                }

                finally
                {
                    UiDocument.Selection.Dispose();
                    CurrentDocument.Regenerate();
                }

                tr.Commit();
                return result;
            }
        }

        // Internal method that allows this class to use this private fields it contains
        // without having to set them necessarily.
        protected Result InternalExecute()
        {
            try
            {
                // defined in derived classes
                return Work();
            }

            catch(Exception e)
            {
                if (e.Message == "The user aborted the pick operation.")
                    return Result.Cancelled;

                Debug.WriteLine("Command failed because of an unknown exception");
                TaskDialog.Show("Command Failed",
                    "There was an error behind the scenes that caused the command to fail horribly and die.");
            }

            return Result.Failed;
        }

        protected abstract Result Work();
    }
}