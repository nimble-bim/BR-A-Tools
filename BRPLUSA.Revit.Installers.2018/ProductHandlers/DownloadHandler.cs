using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class ProductDownloadHandler
    {
        private UpdateManager UpdateManager { get; set; }
        public ProductDownloadHandler(UpdateManager mgr)
        {
            UpdateManager = mgr;
        }

        public async Task InitializeProductState()
        {

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