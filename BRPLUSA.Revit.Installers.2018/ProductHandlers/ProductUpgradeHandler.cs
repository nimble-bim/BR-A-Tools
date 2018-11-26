using System;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class ProductUpgradeHandler : BaseProductHandler
    {
        public ProductUpgradeHandler(UpdateManager mgr, FileReplicationService frp)
            : base(mgr, frp) { }

        public async Task<bool> HandleProductUpgrade(ProductVersionHandler vHandler, ProductDownloadHandler dHandler)
        {
            if (!vHandler.ShouldUpdate)
                return true;

            var info = vHandler.GetVersionInformationFromServer();
            await dHandler.DownloadNewReleases(info.Result.ReleasesToApply);
            var success = await ApplyUpgrade(info);

            return success;
        }

        public async Task<bool> ApplyUpgrade(UpdateInfo info)
        {
            try
            {
                var version = await UpdateManager.ApplyReleases(info);

                return true;
            }

            catch (Exception e)
            {
                throw new Exception("Failed to apply releases", e);
            }
        }
    }
}
