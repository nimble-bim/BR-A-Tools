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
        private string UpdatePath { get; set; }
        private FileInstallHandler InstallHandler { get; set; }

        public FileSystemHandler(string path)
        {
            Initialize(path);
        }

        private void Initialize(string path)
        {
            InstallHandler = new FileInstallHandler();
            UpdatePath = path;
        }

        public async Task HandleRevit2018Installation(string tempDir, string destDir = null)
        {
            var v2018 = destDir ?? RevitAddinLocationProvider.GetRevitAddinFolderLocation(RevitVersion.V2018);
            var finalDir = Path.Combine(v2018, "BRPLUSA");

            var files = await Task.Run(() => Directory.EnumerateFiles(tempDir, "*", SearchOption.AllDirectories).ToArray());
            InstallHandler.HandleFileInstallation(files, finalDir);
        }
    }
}
