using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class InstallHandler : BaseProductHandler
    {
        public InstallHandler(UpdateManager mgr, FileReplicationService frp)
            : base(mgr, frp) { }

        public void ConfigureAppInstallation()
        {
            var appDir = UpdateManager.RootAppDirectory;

            SquirrelAwareApp.HandleEvents(
                onInitialInstall: v =>
                {
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                    UpdateManager.RemoveShortcutForThisExe();
                    FileReplicationService.ReplicateFilesToRevitLocations(appDir);
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                },
                onAppUpdate: v => {
                    UpdateManager.RemoveShortcutForThisExe();
                    FileReplicationService.ReplicateFilesToRevitLocations(appDir);
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
                    FileReplicationService.ReplicateFilesToRevitLocations(appDir);
                    UpdateManager.KillAllExecutablesBelongingToPackage();
                }
            );
        }
    }
}