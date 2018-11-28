using System;
using System.Threading.Tasks;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class ProductInstallHandler : BaseProductHandler
    {
        public ProductInstallHandler(UpdateManager mgr, FileReplicationService frp)
            : base(mgr, frp) { }

        public void ConfigureAppInstallation()
        {
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: v =>
                {
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                    UpdateManager.RemoveShortcutForThisExe();
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                },
                onAppUpdate: v => {
                    UpdateManager.RemoveShortcutForThisExe();
                },
                onAppUninstall: async v => {
                    UpdateManager.RemoveShortcutForThisExe();
                    await UpdateManager.FullUninstall();
                    FileReplicationService.CleanUpReplicationDirectory();
                },
                onFirstRun: () =>
                {
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                    UpdateManager.RemoveShortcutForThisExe();
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                }
            );
        }

        public async Task<bool> HandleInitialInstallation(ProductVersionHandler vHandler, ProductDownloadHandler dHandler)
        {
            //ConfigureAppInstallation();

            return await HandleProductInstallation(vHandler, dHandler);
        }

        public async Task<bool> HandleProductInstallation(ProductVersionHandler vHandler, ProductDownloadHandler dHandler)
        {
            if (vHandler.HasChecked && !vHandler.ShouldUpdate)
                return true;

            var info = await vHandler.GetVersionInformationFromServer();
            await dHandler.DownloadNewReleases(info.ReleasesToApply);
            var success = await ApplyNewProduct(info);

            var appDir = UpdateManager.RootAppDirectory;
            FileReplicationService.ReplicateFilesToRevitLocations(appDir);

            return success;
        }

        public async Task<bool> ApplyNewProduct(UpdateInfo info)
        {
            try
            {
                var version = await UpdateManager.ApplyReleases(info);

                return true;
            }

            catch (Exception e)
            {
                LoggingService.LogError("Failed to apply releases", e);
                throw new Exception("Failed to apply releases", e);
                
            }
        }
    }
}