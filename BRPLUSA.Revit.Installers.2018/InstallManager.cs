using System;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.Services;

namespace BRPLUSA.Revit.Installers._2018
{
    /// <summary>
    /// This holds the status of the installation and manages
    /// the process of installing and upgrading the product
    /// </summary>
    public class InstallManager : IDisposable
    {
        public bool Revit2018IsInstalled { get; private set; }
        public bool Revit2018AppUpdateAvailable { get; private set; }
        public bool Revit2018AppInstalled { get; private set; }
        private InstallHandlingService InstallHandler { get; set; }
        private InstallStatusService StatusService {get; set;}

        public InstallManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializeStatusService();
            DoPreInstallStatusCheck();

            if(Revit2018IsInstalled)
                InitializeHandlers();
        }

        private void InitializeStatusService()
        {
            StatusService = new InstallStatusService();
        }

        private void DoPreInstallStatusCheck()
        {
            Revit2018IsInstalled = StatusService.IsRevit2018Installed;
        }

        private void InitializeHandlers()
        {
            InstallHandler = new InstallHandlingService();
        }

        public async Task InitializeProductState()
        {
            await InstallHandler.InitializeProductState();
            SetInstallationStatuses();
        }

        private void SetInstallationStatuses()
        {
            Revit2018AppInstalled = InstallHandler.Revit2018AppInstalled;
            Revit2018AppUpdateAvailable = InstallHandler.Revit2018AppUpdateAvailable;
        }

        public async Task<bool> HandleRevit2018ApplicationUpgrade()
        {
            var success = await InstallHandler.HandleRevit2018Upgrade();

            return success;
        }

        public async Task<bool> HandleRevit2018ApplicationInstallation()
        {
            var success = await InstallHandler.HandleRevit2018Installation();

            return success;
        }

        public void Dispose()
        {
            InstallHandler.Dispose();
        }
    }
}
