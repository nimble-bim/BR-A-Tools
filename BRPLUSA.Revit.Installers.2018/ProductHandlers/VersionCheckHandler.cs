using System;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.Entities;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class ProductVersionHandler
    {
        private UpdateManager UpdateManager { get; set; }
        public VersionData LocalVersion { get; private set; }
        public VersionData ServerVersion { get; private set; }
        public bool Revit2018UpdateAvailable { get; private set; }
        public bool HasCheckedForUpdate { get; private set; }

        public ProductVersionHandler(UpdateManager mgr)
        {
            Initialize(mgr);
        }

        private void Initialize(UpdateManager mgr)
        {
            UpdateManager = mgr;
            GetVersionInformationFromServer().Wait();
        }

        public async Task<UpdateInfo> GetVersionInformationFromServer()
        {
            try
            {
                var info = await UpdateManager.CheckForUpdate();

                LocalVersion = GetVersionData(info?.CurrentlyInstalledVersion);
                ServerVersion = GetVersionData(info?.FutureReleaseEntry);
                Revit2018UpdateAvailable = LocalVersion < ServerVersion;
                HasCheckedForUpdate = true;

                return info;
            }

            catch (Exception e)
            {
                throw new Exception("Failed to retrieve information", e);
            }
        }

        public VersionData GetVersionData(ReleaseEntry info)
        {
            var version = VersionService.GetVersionInformation(info);

            return version;
        }
    }
}