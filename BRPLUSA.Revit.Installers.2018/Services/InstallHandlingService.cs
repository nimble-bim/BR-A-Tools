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
        private AppVersionHandler VersionHandler { get; set; }
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

        public async Task InitializeProductState()
        {
            VersionHandler = new AppVersionHandler(UpdateManager);
            DownloadHandler = new AppDownloadHandler(UpdateManager);
            FileHandler = new FileSystemHandler(UpdateManager);

            await VersionHandler.InitializeProductState();
            await DownloadHandler.InitializeProductState();
            await FileHandler.InitializeProductState();

            Revit2018AppInstalled = await IsAppForRevit2018Installed();
            Revit2018AppUpdateAvailable = CheckForRevit2018AppUpdates();
        }

        private async Task<bool> IsAppForRevit2018Installed()
        {
            return await InstallStatusService.IsAppForRevit2018Installed();
        }

        private bool CheckForRevit2018AppUpdates()
        {
            return VersionHandler.Revit2018UpdateAvailable;
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
            try
            {
                var info = await VersionHandler.GetVersionInformationFromServer();
                await DownloadHandler.DownloadNewReleases(info.ReleasesToApply);
                var tempLocation = await PushNewReleaseToTempLocation(info);

                await FileHandler.HandleRevit2018Installation(tempLocation);

                return true;
            }

            catch (Exception e)
            {
                LoggingService.LogError("Unable to install product because of fatal error", e);
            }

            return false;
        }

        public async Task<bool> HandleRevit2018Upgrade()
        {
            return true;
        }

        public async Task<string> PushNewReleaseToTempLocation(UpdateInfo info)
        {
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