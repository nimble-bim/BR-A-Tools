using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Mechanical;
using BRPLUSA_Tools.Commands;

namespace BRPLUSA_Tools
{
    public class RevitApplicationEnhancements : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            return Initialize(app);
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Uninitialize(app);
        }

        private Result Initialize(UIControlledApplication app)
        {
            try
            {
                CreateRibbon(app);
                var updater = new SpatialPropertyUpdater(app.ActiveAddInId);
                UpdaterRegistry.RegisterUpdater(updater);
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error occuring: {e.Message}");
                return Result.Failed;
            }
        }

        private Result Uninitialize(UIControlledApplication app)
        {
            try
            {
                var updater = new SpatialPropertyUpdater(app.ActiveAddInId);
                UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId());
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error occuring: {e.Message}");
                return Result.Failed;
            }
        }

        public void CreateRibbon(UIControlledApplication app)
        {
            app.CreateRibbonTab("BR+A");

            var brpa = app.CreateRibbonPanel("BR+A", "Utilities");
            
            var spaceSync = new PushButtonData("Link Spaces", "Link Spaces", typeof(SpatialLink).Assembly.Location, typeof(SpatialLink).FullName);

            brpa.AddItem(spaceSync);
        }
    }
}
