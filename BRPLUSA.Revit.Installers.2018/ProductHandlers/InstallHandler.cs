using System;
using System.Threading.Tasks;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class ProductInstallHandler : BaseProductHandler
    {
        public ProductInstallHandler(UpdateManager mgr, FileInstallationService frp)
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
            try
            {
                if (vHandler.HasChecked && !vHandler.ShouldUpdate)
                    return true;

                var info = await vHandler.GetVersionInformationFromServer();
                await dHandler.DownloadNewReleases(info.ReleasesToApply);
                var tempLocation = PushNewReleaseToTempLocation(info).Result;

                FileReplicationService.InstallOnFileSystem(tempLocation);

                return true;
            }

            catch (Exception e)
            {
                LoggingService.LogError("Unable to install product because of fatal error", e);
            }

            return false;
        }

        public async Task<string> PushNewReleaseToTempLocation(UpdateInfo info)
        {
            try
            {
                var location = await UpdateManager.ApplyReleases(info);

                return location;
            }

            catch (Exception e)
            {
                LoggingService.LogError("Failed to apply releases", e);
                throw new Exception("Failed to apply releases", e);
                
            }
        }
    }
}