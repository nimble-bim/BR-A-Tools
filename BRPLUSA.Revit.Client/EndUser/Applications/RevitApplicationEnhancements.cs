using System;
using System.Diagnostics;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Client.EndUser.Commands;
using BRPLUSA.Revit.Client.EndUser.Services;
using BRPLUSA.Revit.Client.UI.Views;
using BRPLUSA.Revit.Services;
using BRPLUSA.Revit.Services.Updates;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Client.EndUser.Applications
{
    // if this doesn't debug - check this link:
    // https://blogs.msdn.microsoft.com/devops/2013/10/16/switching-to-managed-compatibility-mode-in-visual-studio-2013/

    public class RevitApplicationEnhancements : IExternalApplication
    {
        public BardWebClient Sidebar { get; set; }
        private UIControlledApplication CurrentApplication { get; set; }
        private SocketService SocketService { get; set; }

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
                CurrentApplication = app;

                ResolveBrowserBinaries();
                InitializeSocketService();
                CreateRibbon();
                RegisterSideBar();

                UpdaterRegistrationService.AddRegisterableServices(
                    //new SpatialPropertyUpdater(app),
                    new AutoModelBackupServices(),
                    new ManualModelBackupServices(SocketService)
                    );

                app.ControlledApplication.DocumentOpened += UpdaterRegistrationService.RegisterServices;
                app.ControlledApplication.DocumentClosed += UpdaterRegistrationService.DeregisterServices;

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
                app.ControlledApplication.DocumentClosed -= UpdaterRegistrationService.DeregisterServices;
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error occuring: {e.Message}");
                return Result.Failed;
            }
        }

        public void CreateRibbon()
        {
            CurrentApplication.CreateRibbonTab("BR+A");

            var brpa = CurrentApplication.CreateRibbonPanel("BR+A", "Utilities");
            
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

        public void InitializeSocketService(string url = "http://localhost:4422")
        {
            // TODO: upgrade to factory at some point
            var serv = new SocketService();
            SocketService = serv;
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

        private void RegisterSideBar()
        {
            Sidebar = new BardWebClient(SocketService);
            Sidebar.RegisterEvents(CurrentApplication);

            CurrentApplication.RegisterDockablePane(BardWebClient.Id, "BR+A Revit Helper", Sidebar);
        }
    }
}
