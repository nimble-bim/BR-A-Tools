using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class AppDownloadHandler
    {
        private UpdateManager UpdateManager { get; set; }
        public AppDownloadHandler(UpdateManager mgr)
        {
            UpdateManager = mgr;
        }

        public async Task InitializeProductState()
        {
            LoggingService.LogInfo("Initializing AppDownloadHandler");
            LoggingService.LogInfo("Initialized AppDownloadHandler");
        }

        public async Task DownloadNewReleases(IEnumerable<ReleaseEntry> releases)
        {
            try
            {
                await UpdateManager.DownloadReleases(releases);
            }

            catch (Exception e)
            {
                throw new Exception("Failed to download releases", e);
            }
        }
    }
}