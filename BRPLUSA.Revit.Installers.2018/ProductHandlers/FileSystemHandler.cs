using System.IO;
using System.Linq;
using BRPLUSA.Revit.Installers._2018.Providers;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class FileSystemHandler
    {
        private FileInstallHandler InstallHandler { get; set; }

        public FileSystemHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            InstallHandler = new FileInstallHandler();
        }

        public void HandleRevit2018Installation(string tempDir, string destDir = null)
        {
            var v2018 = destDir ?? RevitAddinLocationProvider.GetRevitAddinFolderLocation(RevitVersion.V2018);
            var finalDir = Path.Combine(v2018, "BRPLUSA");

            var files = Directory.EnumerateFiles(tempDir, "*", SearchOption.AllDirectories).ToArray();
            InstallHandler.HandleFileInstallation(files, finalDir);
        }

        public void CleanUpReplicationDirectory()
        {

        }
    }
}
