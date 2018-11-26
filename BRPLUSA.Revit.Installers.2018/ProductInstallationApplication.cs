using Squirrel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.ProductHandlers;
using BRPLUSA.Revit.Installers._2018.Services;
using BRPLUSA.Revit.Installers._2018.Views;

namespace BRPLUSA.Revit.Installers._2018
{
    public class ProductInstallationApplication
    {
        private string LocalPath { get; set; }
        private string ServerPath { get; set; }
        private UpdateManager UpdateManager { get; set; }
        private FileReplicationService FileReplicationService { get; set; }
        public InstallHandler InstallHandler { get; private set; }
        public ProductUpgradeHandler UpdateHandler { get; private set; }
        public DownloadHandler DownloadHandler { get; private set; }
        public VersionCheckHandler VersionHandler { get; private set; }

        public ProductInstallationApplication()
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

            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            InstallHandler = new InstallHandler();
            UpdateHandler = new ProductUpgradeHandler();
            DownloadHandler = new DownloadHandler();
            VersionHandler = new VersionCheckHandler();
        }

        public async Task<ReleaseEntry> HandleApplicationUpgrade()
        {
            var info = await GetVersionInformationFromServer();

            await DownloadNewReleases(info.ReleasesToApply);
            await ApplyUpgrade(info);

            return info.FutureReleaseEntry;
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
