using System;
using System.Diagnostics;
using Autodesk.Revit.UI;
using BRPLUSA.Client.Commands;
using BRPLUSA.Client.Updaters;
using BRPLUSA.Services;

namespace BRPLUSA.Client.Applications
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
            
            var spaceSync = new PushButtonData("Link Spaces", "Link Spaces", typeof(LinkSpaces).Assembly.Location, typeof(LinkSpaces).FullName);
            var spaceDeSync = new PushButtonData("Unlink Spaces", "Unlink Spaces", typeof(UnlinkSpaces).Assembly.Location, typeof(UnlinkSpaces).FullName);

            brpa.AddItem(spaceSync);
            brpa.AddItem(spaceDeSync);
        }
    }
}
