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
        public bool Revit2018Installed { get; private set; }
        public bool AppFor2018HasUpdateAvailable { get; private set; }
        public bool AppFor2018Installed { get; private set; }
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

            if(Revit2018Installed)
                InitializeHandlers();
        }

        private void InitializeStatusService()
        {
            StatusService = new InstallStatusService();
        }

        private void DoPreInstallStatusCheck()
        {
            Revit2018Installed = StatusService.IsRevit2018Installed;
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
            AppFor2018Installed = InstallHandler.Revit2018AppInstalled;
            AppFor2018HasUpdateAvailable = InstallHandler.Revit2018AppUpdateAvailable;
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
