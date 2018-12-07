using System;
using System.Threading.Tasks;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018.ProductHandlers;
using BRPLUSA.Revit.Installers._2018.Providers;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.Services
{
    public class InstallHandlingService : IDisposable
    {
        private string LocalPath { get; set; }
        private string ServerPath { get; set; }
        private UpdateManager UpdateManager { get; set; }
        private FileSystemHandler FileHandler { get; set; }
        private AppDownloadHandler DownloadHandler { get; set; }

        public bool Revit2018AppInstalled { get; set; }
        public bool Revit2018AppUpdateAvailable { get; set; }

        public InstallHandlingService(bool useLocalFiles = false)
        {
            LoggingService.LogInfo("Initializing Install Handling Service");
            Initialize(useLocalFiles);
            LoggingService.LogInfo("Initialized Install Handling Service");
        }

        private void Initialize(bool useLocalFiles = false)
        {
            ServerPath = "https://app-brplusa-release.s3.amazonaws.com";
            LocalPath = UpdateManager.GetLocalAppDataDirectory() + @"\BRPLUSA\ProductVersions";

            UpdateManager = new UpdateManager(
                useLocalFiles
                    ? LocalPath
                    : ServerPath);

            LoggingService.LogInfo(useLocalFiles 
                ? "Update Manager is using local path" 
                : "Update Manager is using server path");

            LoggingService.LogInfo("Starting app installation configuration");
            ConfigureAppInstallation();
            LoggingService.LogInfo("Completed app installation configuration");
        }

        public async Task InitializeAppState()
        {
            DownloadHandler = new AppDownloadHandler(UpdateManager);
            FileHandler = new FileSystemHandler(UpdateManager);

            Revit2018AppInstalled = await CheckIf2018AppIsInstalledAsync();
            Revit2018AppUpdateAvailable = await CheckForUpdateTo2018AppAsync();
        }

        private bool CheckIf2018AppIsInstalled()
        {
            return CheckIf2018AppIsInstalledAsync().Result;
        }

        private async Task<bool> CheckIf2018AppIsInstalledAsync()
        {
            return await InstallStatusService.CheckIf2018AppIsInstalledAsync();
        }

        private bool CheckForUpdateTo2018App()
        {
            return CheckForUpdateTo2018AppAsync().Result;
        }

        private async Task<bool> CheckForUpdateTo2018AppAsync()
        {
            return await InstallStatusService.CheckForUpdateTo2018AppAsync(UpdateManager);
        }

        public void ConfigureAppInstallation()
        {
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: v =>
                {
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                    UpdateManager.RemoveShortcutForThisExe();
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                },
                onAppUpdate: v => {
                    UpdateManager.RemoveShortcutForThisExe();
                },
                onAppUninstall: async v => {
                    UpdateManager.RemoveShortcutForThisExe();
                    await UpdateManager.FullUninstall();
                    //FileHandler.CleanUpReplicationDirectory();
                },
                onFirstRun: () =>
                {
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                    UpdateManager.RemoveShortcutForThisExe();
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                }
            );
        }

        public async Task<bool> HandleRevit2018Installation()
        {
            LoggingService.LogInfo("Beginning app installation...");
            try
            {
                var info = await InstallStatusService.GetVersionInformationFromServerAsync(UpdateManager);
                await DownloadHandler.DownloadNewReleases(info.ReleasesToApply);
                var tempLocation = await PushNewReleaseToTempLocation(info);

                await FileHandler.HandleRevit2018Installation(tempLocation);

                LoggingService.LogInfo("App installation complete");
                return true;
            }

            catch (Exception e)
            {
                LoggingService.LogError("Unable to install product because of fatal error", e);
            }

            LoggingService.LogInfo("App installation failed");
            return false;
        }

        public async Task<string> PushNewReleaseToTempLocation(UpdateInfo info)
        {
            LoggingService.LogInfo("Pushing release to temporary location...");
            try
            {
                var location = await UpdateManager.ApplyReleases(info);

                return location;
            }

            catch (Exception e)
            {
                LoggingService.LogError("Failed to apply releases", e);
                throw new Exception("Failed to apply releases", e);
                
            }
        }

        public void Dispose()
        {
            UpdateManager.Dispose();
        }
    }
}