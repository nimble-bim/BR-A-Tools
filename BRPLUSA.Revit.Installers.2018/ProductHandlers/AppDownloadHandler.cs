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

        public async Task DownloadNewReleases(IEnumerable<ReleaseEntry> releases)
        {
            LoggingService.LogInfo("Beginning app release download");
            try
            {
                await UpdateManager.DownloadReleases(releases);
            }

            catch (Exception e)
            {
                throw new Exception("Failed to download releases", e);
            }

            LoggingService.LogInfo("App release download complete");
        }
    }
}