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
        private string UpdatePath { get; set; }
        public AppDownloadHandler(string path)
        {
            UpdatePath = path;
        }

        public async Task DownloadNewReleases(IEnumerable<ReleaseEntry> releases)
        {
            LoggingService.LogInfo("Beginning app release download");
            using (var mgr = new UpdateManager(UpdatePath))
            {
                try
                {
                    await mgr.DownloadReleases(releases);
                }

                catch (Exception e)
                {
                    throw new Exception("Failed to download releases", e);
                }
            }

            LoggingService.LogInfo("App release download complete");
        }
    }
}