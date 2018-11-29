using Squirrel;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.ProductHandlers;
using BRPLUSA.Revit.Installers._2018.Services;

namespace BRPLUSA.Revit.Installers._2018
{
    public class InstallationManager
    {
        private string LocalPath { get; set; }
        private string ServerPath { get; set; }
        private UpdateManager UpdateManager { get; set; }
        private FileInstallationService FileReplicationService { get; set; }
        public ProductInstallHandler InstallHandler { get; private set; }
        public ProductUpgradeHandler UpgradeHandler { get; private set; }
        public ProductDownloadHandler DownloadHandler { get; private set; }
        public ProductVersionHandler VersionHandler { get; private set; }

        public InstallationManager()
        {
            Initialize();
        }

        private void Initialize(bool useLocalFiles = false)
        {
            LocalPath = UpdateManager.GetLocalAppDataDirectory() + @"\BRPLUSA\ProductVersions";
            ServerPath = "https://app-brplusa-release.s3.amazonaws.com";

            UpdateManager = new UpdateManager(
                useLocalFiles
                    ? LocalPath
                    : ServerPath);

            FileReplicationService = new FileInstallationService();

            InitializeHandlers(UpdateManager, FileReplicationService);

            InstallHandler.ConfigureAppInstallation();
        }

        private void InitializeHandlers(UpdateManager mgr, FileInstallationService frp)
        {
            InstallHandler = new ProductInstallHandler(mgr, frp);
            UpgradeHandler = new ProductUpgradeHandler(mgr, frp);
            DownloadHandler = new ProductDownloadHandler(mgr, frp);
            VersionHandler = new ProductVersionHandler(mgr, frp);
        }

        public async Task<bool> HandleRevit2018ApplicationUpgrade()
        {
            var success = await UpgradeHandler.HandleRevit2018Upgrade(VersionHandler, DownloadHandler, InstallHandler);

            return success;
        }

        public async Task<bool> HandleRevit2018ApplicationInstallation()
        {
            var success = await InstallHandler.HandleRevit2018Installation(VersionHandler, DownloadHandler);

            return success;
        }
    }
}
