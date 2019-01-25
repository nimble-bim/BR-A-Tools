using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Client.EndUser.Commands;
using BRPLUSA.Revit.Client.EndUser.Commands.Mechanical;
using BRPLUSA.Revit.Client.EndUser.Commands.VAVServes;
using BRPLUSA.Revit.Client.EndUser.Services;
using BRPLUSA.Revit.Client.WPF;
using BRPLUSA.Revit.Client.WPF.Viewers;
using BRPLUSA.Revit.Installers._2018;
using BRPLUSA.Revit.Services.Registration;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Client.EndUser.Applications
{
    // if this doesn't debug - check this link:
    // https://blogs.msdn.microsoft.com/devops/2013/10/16/switching-to-managed-compatibility-mode-in-visual-studio-2013/
    public class RevitApplicationEnhancements : IExternalApplication
    {
        private static SocketService SocketService { get; set; }
        private static AppInstallClient InstallApp { get; set; }
        private UIControlledApplication UiApplication { get; set; }

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
                LoggingService.LogInfo("Starting up application via Revit");
                ResolveBrowserBinaries();
                ResolveUIBinaries();

                var backupAuto = new AutoModelBackupService();
                var backupManual = new ManualModelBackupService();
                var sidebar = new BardWpfClient();

                UpdaterRegistrationService.AddRegisterableServices(
                    backupAuto
                    );

                SocketRegistrationService.AddRegisterableServices(
                    backupManual
                    );

                CreateRibbon(app);
                RegisterAppEvents(app);
                RegisterSideBar(app, sidebar);
                RegisterInstallerEvents(app);

                LoggingService.LogInfo("Application loaded successfully");
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                LoggingService.LogError("Failed to load application due to internal error:", e);
                TaskDialog.Show("Fatal error", $"Failed to load application due to internal exception: {e.Message}");
                return Result.Failed;
            }
        }

        private Result Uninitialize(UIControlledApplication app)
        {
            try
            {
                LoggingService.LogInfo("Shutting down application via Revit");
                HandleServiceDeregistration(app);
                HandleApplicationUpdate();
                LoggingService.LogInfo("Application shutdown complete!");
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                LoggingService.LogError("Failure to shut down correctly", e);
                return Result.Failed;
            }
        }

        private void RegisterAppEvents(UIControlledApplication app)
        {
            app.ControlledApplication.DocumentOpened += UpdaterRegistrationService.RegisterServices;
            app.ControlledApplication.DocumentOpened += SocketRegistrationService.RegisterServices;
            app.ControlledApplication.DocumentClosed += UpdaterRegistrationService.DeregisterServices;
            app.ControlledApplication.DocumentClosed += SocketRegistrationService.DeregisterServices;
        }

        private void HandleServiceDeregistration(UIControlledApplication app)
        {
            app.ControlledApplication.DocumentOpened -= UpdaterRegistrationService.RegisterServices;
            app.ControlledApplication.DocumentOpened -= SocketRegistrationService.RegisterServices;
            app.ControlledApplication.DocumentClosed -= UpdaterRegistrationService.DeregisterServices;
            app.ControlledApplication.DocumentClosed -= SocketRegistrationService.DeregisterServices;
        }

        private void RegisterInstallerEvents(UIControlledApplication app)
        {
            UiApplication = app;
            UiApplication.Idling += CheckUpdateDuringIdling;
        }

        private void CheckUpdateDuringIdling(object sender, IdlingEventArgs args)
        {
            LoggingService.LogInfo("Initializing application to check for product updates");
            UiApplication.Idling -= CheckUpdateDuringIdling;
            InstallApp = new AppInstallClient(true);
        }

        private void HandleApplicationUpdate()
        {
            try
            {
                // check if app update is necessary
                if (!InstallApp.AppFor2018HasUpdate)
                    return;

                // if so, ask the user if they'd like to update
                var wantsUpdate = PromptUserAboutUpdate();

                // if yes, present the app installer and start it automatically
                if(wantsUpdate)
                    RunUpdateProcess();
            }

            catch (Exception e)
            {
                LoggingService.LogError("Unable to start application update service", e);
            }
        }

        private bool PromptUserAboutUpdate()
        {
            const string title = "BR+A Revit Enhancements Update Available";
            const string msg = "Would you like to update the application?";

            var updateBox = new TaskDialog(title)
            {
                CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No,
                MainContent = msg
            };
            var result = updateBox.Show();

            
            return result == TaskDialogResult.Yes;
        }

        private void RunUpdateProcess()
        {
            LoggingService.LogInfo("Product update application initialized and ready to run");
            InstallApp.Reveal();
            InstallApp.Run();
            LoggingService.LogInfo("Product update application process completed");
        }

        public void CreateRibbon(UIControlledApplication app)
        {
            try
            {
                LoggingService.LogInfo("Creating Application ribbon within Revit");
                app.CreateRibbonTab("BR+A");

                var brpa = app.CreateRibbonPanel("BR+A", "Utilities");
                var toggleSidebar = new PushButtonData("Toggle Sidebar", "Toggle Sidebar",
                    typeof(ShowSidebar).Assembly.Location, typeof(ShowSidebar).FullName);
                //var spaceSync = new PushButtonData("Link Spaces", "Link Spaces", typeof(LinkSpaces).Assembly.Location, typeof(LinkSpaces).FullName);
                //var exportAreaToNavis = new PushButtonData("Export Area To Navisworks", "Clash Area", typeof(ExportAreaToNavis).Assembly.Location, typeof(ExportAreaToNavis).FullName);
                var findElement = new PushButtonData("Find Element By Name", "Find Element",
                    typeof(SelectByName).Assembly.Location, typeof(SelectByName).FullName);
                //var findPanel = new PushButtonData("Find Panel By Name", "Find Panel", typeof(SelectPanelFromSchedule).Assembly.Location, typeof(SelectPanelFromSchedule).FullName);
                var switchType = new PushButtonData("Switch Element Type", "Switch Element Type",
                    typeof(ForceTypeSwap).Assembly.Location, typeof(ForceTypeSwap).FullName);
                var createVentSchedule = new PushButtonData("Create Vent Schedule", "Create Vent Schedule",
                    typeof(CreateVentilationRequirementsScheduleCommand).Assembly.Location,
                    typeof(CreateVentilationRequirementsScheduleCommand).FullName);
                var vavServes = new PushButtonData("Show What VAV Serves", "VAV Serves", 
                    typeof(ShowRoomsServedByVAV).Assembly.Location, 
                    typeof(ShowRoomsServedByVAV).FullName);


                brpa.AddItem(toggleSidebar);
                //brpa.AddItem(spaceSync);
                //brpa.AddItem(exportAreaToNavis);
                brpa.AddItem(findElement);
                //brpa.AddItem(findPanel);
                brpa.AddItem(switchType);
                brpa.AddItem(createVentSchedule);
                brpa.AddItem(vavServes);
                LoggingService.LogInfo("Application Ribbon creation complete");
            }

            catch (Exception e)
            {
                throw new Exception("Failed to create application ribbon within Revit", e);
            }
        }

        public void ResolveBrowserBinaries()
        {
            //try
            //{
            //    LoggingService.LogInfo("Attempting to resolve browser binaries");

            //    AppDomain.CurrentDomain.AssemblyResolve += BardWebClient.ResolveCefBinaries;
            //    BardWebClient.InitializeCefSharp();

            //    LoggingService.LogInfo("Browser binary resolution complete");
            //}

            //catch (Exception e)
            //{
            //    var ex = new Exception("Fatal error! Failed to resolve browser binaries", e);

            //    throw ex;
            //}
        }

        public void ResolveUIBinaries()
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    if (!args.Name.StartsWith("Dragablz") && !args.Name.StartsWith("MaterialDesign"))
                        return null;

                    string assName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                    var location = Path.GetDirectoryName(typeof(RevitApplicationEnhancements).Assembly.Location);
                    var path = Path.Combine(location, assName);

                    return File.Exists(path)
                        ? Assembly.LoadFile(path)
                        : null;
                };
            }

            catch(Exception e)
            {
                var ex = new Exception("Fatal error! Failed to resolve UI binaries", e);

                throw ex;
            }
        }

        private void RegisterSideBar(UIControlledApplication app, BardWpfClient sidebar)
        {
            try
            {
                LoggingService.LogInfo("Attempting to register Bard Client sidebar with Revit");

                app.RegisterDockablePane(BardWpfClient.Id, "BR+A Revit Helper", sidebar);
                //app.ControlledApplication.DocumentOpened += sidebar.JoinRevitSession;
                app.ControlledApplication.DocumentOpened += sidebar.ShowSidebar;

                LoggingService.LogInfo("Sidebar registered with client");
            }

            catch (Exception e)
            {
                throw new Exception("Failed to resolve browser binaries", e);
            }
        }
    }
}
