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

        public async Task<string> ApplyUpgrade(UpdateInfo info)
        {
            try
            {
                var version = await UpdateManager.ApplyReleases(info);

                return version;
            }

            catch (Exception e)
            {
                throw new Exception("Failed to apply releases", e);
            }
        }
    }
}
