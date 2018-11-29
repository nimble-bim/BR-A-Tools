using System.IO;
using System.Linq;
using BRPLUSA.Revit.Installers._2018.Providers;
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
            InitializeProductState();
        }

        private void InitializeProductState()
        {
            IsRevit2018AppInstalled = CheckForRevit2018AppInstallations();
        }

        public void HandleRevit2018Installation(string tempDir, string destDir = null)
        {
            var v2018 = destDir ?? RevitAddinLocationProvider.GetRevitAddinFolderLocation(RevitVersion.V2018);
            var finalDir = Path.Combine(v2018, "BRPLUSA");

            var files = Directory.EnumerateFiles(tempDir, "*", SearchOption.AllDirectories).ToArray();
            InstallHandler.HandleFileInstallation(files, finalDir);
        }

        private bool CheckForRevit2018AppInstallations()
        {
            if (UpdateManager?.CurrentlyInstalledVersion() != null)
                return true;

            var v2018 = RevitAddinLocationProvider.GetRevitAddinFolderLocation(RevitVersion.V2018);
            var addinFiles = Directory.EnumerateFiles(v2018).ToArray();
            var v2018installed = addinFiles.Any(fileName => fileName.Equals("BRPLUSA.addin"));

            return v2018installed;
        }
    }
}
