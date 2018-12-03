using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.Providers;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class FileSystemHandler
    {
        private UpdateManager UpdateManager { get; set; }
        private FileInstallHandler InstallHandler { get; set; }

        public bool IsRevit2018AppInstalled {get; set; }

        public FileSystemHandler(UpdateManager mgr)
        {
            Initialize(mgr);
        }

        private void Initialize(UpdateManager mgr)
        {
            InstallHandler = new FileInstallHandler();
            UpdateManager = mgr;
        }

        public async Task InitializeProductState()
        {
            IsRevit2018AppInstalled = await IsAppForRevit2018Installed();
        }

        public async Task HandleRevit2018Installation(string tempDir, string destDir = null)
        {
            var v2018 = destDir ?? RevitAddinLocationProvider.GetRevitAddinFolderLocation(RevitVersion.V2018);
            var finalDir = Path.Combine(v2018, "BRPLUSA");

            var files = await Task.Run(() => Directory.EnumerateFiles(tempDir, "*", SearchOption.AllDirectories).ToArray());
            InstallHandler.HandleFileInstallation(files, finalDir);
        }

        private async Task<bool> IsAppForRevit2018Installed()
        {
            return await InstallStatusService.IsAppForRevit2018Installed();
        }
    }
}
