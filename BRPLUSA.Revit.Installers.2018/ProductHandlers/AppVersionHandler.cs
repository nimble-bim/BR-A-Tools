using System;
using System.Threading.Tasks;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018.Entities;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class AppVersionHandler
    {
        private UpdateManager UpdateManager { get; set; }
        public VersionData LocalVersion { get; private set; }
        public VersionData ServerVersion { get; private set; }
        public bool Revit2018UpdateAvailable { get; private set; }
        public bool HasCheckedForUpdate { get; private set; }

        public AppVersionHandler(UpdateManager mgr)
        {
            LoggingService.LogInfo("Initializing AppVersionHandler");
            Initialize(mgr);
            LoggingService.LogInfo("Initialized AppVersionHandler");
        }

        private void Initialize(UpdateManager mgr)
        {
            UpdateManager = mgr;
        }

        public async Task InitializeProductState()
        {
            await GetVersionInformationFromServer();
        }

        public async Task<UpdateInfo> GetVersionInformationFromServer()
        {
            var info = await UpdateManager.CheckForUpdate();

            LocalVersion = GetLocalVersion(info);
            ServerVersion = GetServerVersion(info);

            return info;
        }

        private VersionData GetVersionData(ReleaseEntry info)
        {
            LoggingService.LogInfo("Getting version data...");
            var version = VersionService.GetVersionInformation(info);
            LoggingService.LogInfo($"Version is: {version}");

            return version;
        }

        private VersionData GetLocalVersion(UpdateInfo info)
        {
            LoggingService.LogInfo("Confirming local version data...");
            var local = GetVersionData(info?.CurrentlyInstalledVersion);
            LoggingService.LogInfo("Local version confirmed");

            return local;
        }

        private VersionData GetServerVersion(UpdateInfo info)
        {
            LoggingService.LogInfo("Confirming server version data...");
            var server = GetVersionData(info?.FutureReleaseEntry);
            LoggingService.LogInfo("Server version confirmed");

            return server;
        }
    }
}