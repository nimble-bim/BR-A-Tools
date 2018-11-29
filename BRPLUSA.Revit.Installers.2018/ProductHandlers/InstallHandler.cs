using System;
using System.Threading.Tasks;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class ProductInstallHandler : IDisposable
    {
        private string LocalPath { get; set; }
        private string ServerPath { get; set; }
        private UpdateManager UpdateManager { get; set; }
        private FileSystemHandler FileHandler { get; set; }
        private ProductVersionHandler VersionHandler { get; set; }
        private ProductDownloadHandler DownloadHandler { get; set; }
        public bool Revit2018AppInstalled { get; set; }
        public bool Revit2018AppUpdateAvailable { get; set; }

        public ProductInstallHandler(bool useLocalFiles = false)
        {
            Initialize(useLocalFiles);
        }

        private void Initialize(bool useLocalFiles = false)
        {
            ServerPath = "https://app-brplusa-release.s3.amazonaws.com";
            LocalPath = UpdateManager.GetLocalAppDataDirectory() + @"\BRPLUSA\ProductVersions";

            UpdateManager = new UpdateManager(
                useLocalFiles
                    ? LocalPath
                    : ServerPath);

            ConfigureAppInstallation();
        }

        public async Task InitializeProductState()
        {
            VersionHandler = new ProductVersionHandler(UpdateManager);
            DownloadHandler = new ProductDownloadHandler(UpdateManager);
            FileHandler = new FileSystemHandler(UpdateManager);

            await VersionHandler.InitializeProductState();
            await DownloadHandler.InitializeProductState();
            await FileHandler.InitializeProductState();

            Revit2018AppInstalled = CheckRevit2018AppInstallation();
            Revit2018AppUpdateAvailable = CheckForRevit2018AppUpdates();
        }

        private bool CheckRevit2018AppInstallation()
        {
            return FileHandler.IsRevit2018AppInstalled;
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
                if (VersionHandler.HasCheckedForUpdate && !VersionHandler.Revit2018UpdateAvailable)
                    return true;

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