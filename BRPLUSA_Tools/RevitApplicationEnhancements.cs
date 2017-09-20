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
        private RegistrationService _registerServ;

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
                _registerServ = new RegistrationService(app);
                _registerServ.RegisterServices(new SpatialPropertyUpdater(app));
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
