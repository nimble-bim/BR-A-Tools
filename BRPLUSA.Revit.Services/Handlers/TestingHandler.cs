using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace BRPLUSA.Revit.Services.Handlers
{
    //TODO: sample Handler for POC - create real one later to take care of tasks
    public class TestingHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            TaskDialog.Show("Done", "Completed task!");
        }

        public string GetName()
        {
            return "TestingHandler";
        }
    }
}
