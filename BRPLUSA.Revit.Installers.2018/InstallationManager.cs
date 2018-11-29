using System;
using Squirrel;
using System.Threading.Tasks;
using System.Windows;
using BRPLUSA.Revit.Installers._2018.ProductHandlers;
using BRPLUSA.Revit.Installers._2018.Services;

namespace BRPLUSA.Revit.Installers._2018
{
    /// <summary>
    /// This holds the status of the installation and manages
    /// the process of installing and upgrading the product
    /// </summary>
    public class InstallationManager
    {
        public bool Revit2018AppUpdateAvailable { get; set; }
        public bool Revit2018AppInstalled { get; set; }
        private ProductInstallHandler InstallHandler { get; set; }

        public InstallationManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            InstallHandler = new ProductInstallHandler();
        }

        public async Task InitializeProductState()
        {
            await InstallHandler.InitializeProductState();
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
    }
}
