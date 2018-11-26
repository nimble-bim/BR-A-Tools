using Squirrel;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.ProductHandlers;
using BRPLUSA.Revit.Installers._2018.Services;
using BRPLUSA.Revit.Installers._2018.Views;

namespace BRPLUSA.Revit.Installers._2018
{
    public class InstallationManager
    {
        private string LocalPath { get; set; }
        private string ServerPath { get; set; }
        private UpdateManager UpdateManager { get; set; }
        private FileReplicationService FileReplicationService { get; set; }
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
            ServerPath = "https://app.brplusa.release.s3.amazonaws.com";

            UpdateManager = new UpdateManager(
                useLocalFiles
                    ? LocalPath
                    : ServerPath);

            FileReplicationService = new FileReplicationService();

            InitializeHandlers(UpdateManager, FileReplicationService);
        }

        private void InitializeHandlers(UpdateManager mgr, FileReplicationService frp)
        {
            InstallHandler = new ProductInstallHandler(mgr, frp);
            UpgradeHandler = new ProductUpgradeHandler(mgr, frp);
            DownloadHandler = new ProductDownloadHandler(mgr, frp);
            VersionHandler = new ProductVersionHandler(mgr, frp);
        }

        public async Task<bool> HandleApplicationUpgrade()
        {
            var success = await UpgradeHandler.HandleProductUpgrade(VersionHandler, DownloadHandler);

            return success;
        }

        public async Task<bool> HandleApplicationInstallation()
        {
            return false;
        }

        //public void StartUpdaterApplication()
        //{
        //    var app = new AppInstallClient();
        //    var window = new ProductSelectionView();
        //    app.ShutdownMode = System.Windows.ShutdownMode.OnLastWindowClose;
        //    app.Run(window);
        //}
    }
}
