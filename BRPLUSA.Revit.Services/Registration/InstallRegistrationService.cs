using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018;

namespace BRPLUSA.Revit.Services.Registration
{
    public class InstallRegistrationService
    {
        private static AppInstallClient InstallApp { get; set; }
        private UIControlledApplication UiApplication { get; set; }

        public InstallRegistrationService(UIControlledApplication app)
        {
            RegisterInstallerEvents(app);
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

        public void HandleApplicationUpdate()
        {
            try
            {
                // check if app update is necessary
                if (!InstallApp.AppFor2018HasUpdate)
                    return;

                // if so, ask the user if they'd like to update
                var wantsUpdate = PromptUserAboutUpdate();

                // if yes, present the app installer and start it automatically
                if (wantsUpdate)
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
    }
}
