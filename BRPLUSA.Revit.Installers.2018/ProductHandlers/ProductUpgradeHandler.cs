using System;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class ProductUpgradeHandler : BaseProductHandler
    {
        public ProductUpgradeHandler(UpdateManager mgr, FileInstallationService frp)
            : base(mgr, frp) { }

        public async Task<bool> HandleProductUpgrade(ProductVersionHandler vHandler, ProductDownloadHandler dHandler, ProductInstallHandler iHandler)
        {
            var success = await iHandler.HandleProductInstallation(vHandler, dHandler);

            return success;
        }
    }
}
