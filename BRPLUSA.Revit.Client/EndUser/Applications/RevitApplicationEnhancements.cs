using System;
using System.Diagnostics;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Client.EndUser.Commands;
using BRPLUSA.Revit.Client.EndUser.Services;
using BRPLUSA.Revit.Client.UI.Views;
using BRPLUSA.Revit.Services;
using BRPLUSA.Revit.Services.Registration;
using BRPLUSA.Revit.Services.Updates;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Client.EndUser.Applications
{
    // if this doesn't debug - check this link:
    // https://blogs.msdn.microsoft.com/devops/2013/10/16/switching-to-managed-compatibility-mode-in-visual-studio-2013/

    public class RevitApplicationEnhancements : IExternalApplication
    {
        public BardWebClient Sidebar { get; set; }
        private static SocketService SocketService { get; set; }

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
                ResolveBrowserBinaries();
                CreateRibbon(app);
                RegisterSideBar(app);

                var backupAuto = new AutoModelBackupService();
                var backupManual = new ManualModelBackupService();

                UpdaterRegistrationService.AddRegisterableServices(
                    //new SpatialPropertyUpdater(app),
                    backupAuto
                    );

                SocketRegistrationService.AddRegisterableServices(
                    backupManual
                    );

                app.ControlledApplication.DocumentOpened += UpdaterRegistrationService.RegisterServices;
                app.ControlledApplication.DocumentOpened += SocketRegistrationService.RegisterServices;
                app.ControlledApplication.DocumentClosed += UpdaterRegistrationService.DeregisterServices;
                app.ControlledApplication.DocumentClosed += SocketRegistrationService.DeregisterServices;

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
                app.ControlledApplication.DocumentOpened -= UpdaterRegistrationService.RegisterServices;
                app.ControlledApplication.DocumentOpened -= SocketRegistrationService.RegisterServices;
                app.ControlledApplication.DocumentClosed -= UpdaterRegistrationService.DeregisterServices;
                app.ControlledApplication.DocumentClosed -= SocketRegistrationService.DeregisterServices;
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
            
            //var spaceSync = new PushButtonData("Link Spaces", "Link Spaces", typeof(LinkSpaces).Assembly.Location, typeof(LinkSpaces).FullName);
            //var exportAreaToNavis = new PushButtonData("Export Area To Navisworks", "Clash Area", typeof(ExportAreaToNavis).Assembly.Location, typeof(ExportAreaToNavis).FullName);
            var findElement = new PushButtonData("Find Element By Name", "Find Element", typeof(SelectByName).Assembly.Location, typeof(SelectByName).FullName);
            //var findPanel = new PushButtonData("Find Panel By Name", "Find Panel", typeof(SelectPanelFromSchedule).Assembly.Location, typeof(SelectPanelFromSchedule).FullName);
            var switchType = new PushButtonData("Switch Element Type", "Switch Element Type", typeof(ForceTypeSwap).Assembly.Location, typeof(ForceTypeSwap).FullName);

            //brpa.AddItem(spaceSync);
            //brpa.AddItem(exportAreaToNavis);
            brpa.AddItem(findElement);
            //brpa.AddItem(findPanel);
            brpa.AddItem(switchType);
        }

        public void ResolveBrowserBinaries()
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += BardWebClient.Resolver;
                BardWebClient.InitializeCefSharp();
            }

            catch (Exception e)
            {
                throw new Exception("Couldn't initialize browser", e);
            }
        }

        private void RegisterSideBar(UIControlledApplication app)
        {
            Sidebar = new BardWebClient(SocketService);
            Sidebar.RegisterEvents(app);

            app.RegisterDockablePane(BardWebClient.Id, "BR+A Revit Helper", Sidebar);
        }
    }
}
