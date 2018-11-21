using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Revit.Installers._2018
{
    public class ProductInstallationService
    {
        private readonly string _local = UpdateManager.GetLocalAppDataDirectory() + @"\BRPLUSA\ProductVersions";
        private readonly string _server = "https://app.brplusa.release.s3.amazonaws.com";
        private UpdateManager _mgr;
        private UpdateManager UpdateManager
        {
            get
            {
                if (_mgr == null)
                    Initialize();

                return _mgr;
            }

            set { }
        }

        public ProductInstallationService()
        {
            Initialize();
        }

        private void Initialize(bool useLocalFiles = false)
        {
            _mgr = new UpdateManager(
                useLocalFiles
                    ? _local
                    : _server);
        }
        public void ConfigureAppEvents()
        {
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: v => {
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                    UpdateManager.RemoveShortcutForThisExe();
                    ReplicateFilesToRevitLocations();
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                },
                onAppUpdate: v => {
                    UpdateManager.RemoveShortcutForThisExe();
                    ReplicateFilesToRevitLocations();
                },
                onAppUninstall: async v => {
                    UpdateManager.RemoveShortcutForThisExe();
                    await UpdateManager.FullUninstall();
                    CleanUpReplicationDirectory();
                },
                onFirstRun: () =>
                {
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                    UpdateManager.RemoveShortcutForThisExe();
                    ReplicateFilesToRevitLocations();
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                }
            );
        }

        public async Task<UpdateInfo> GetVersionInformationFromServer()
        {
            try
            {
                using (UpdateManager)
                {
                    var info = await UpdateManager.CheckForUpdate();
                    return info;
                }
            }

            catch (Exception e)
            {
                throw new Exception("Failed to retrieve information", e);
            }
        }

        public async Task DownloadNewReleases(IEnumerable<ReleaseEntry> releases)
        {
            try
            {
                using (UpdateManager)
                {
                    await UpdateManager.DownloadReleases(releases);
                }
            }

            catch (Exception e)
            {
                throw new Exception("Failed to download releases", e);
            }
        }

        public async Task<string> ApplyUpgrade(UpdateInfo info)
        {
            try
            {
                using (UpdateManager)
                {
                    var version = await UpdateManager.ApplyReleases(info);

                    return version;
                }
            }

            catch (Exception e)
            {
                throw new Exception("Failed to apply releases", e);
            }
        }

        public async Task<ReleaseEntry> HandleApplicationUpgrade()
        {
            var info = await GetVersionInformationFromServer();

            await DownloadNewReleases(info.ReleasesToApply);
            await ApplyUpgrade(info);

            return info.FutureReleaseEntry;
        }

        private void ReplicateFilesToRevitLocations()
        {
            ReplicateFilesToRevit2018Location();
            //ReplicateFilesToRevit2019Location();
        }

        private void ReplicateFilesToRevit2018Location()
        {
            var appDir = UpdateManager.RootAppDirectory;
            FileReplicationService.ReplicateFilesToRevit2018AddinLocation(appDir);
        }

        private void CleanUpReplicationDirectory()
        {
            FileReplicationService.CleanUpReplicationDirectory();
        }

        public bool IsApplicationUpdateNecessary()
        {
            return true;
        }

        public void StartUpdaterApplication()
        {
            var app = new App();
            var window = new MainWindow();
            app.ShutdownMode = System.Windows.ShutdownMode.OnLastWindowClose;
            app.Run(window);
        }
    }
}
