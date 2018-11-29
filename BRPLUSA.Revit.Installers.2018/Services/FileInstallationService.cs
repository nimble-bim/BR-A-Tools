using System;
using System.IO;
using System.Linq;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018.Providers;

namespace BRPLUSA.Revit.Installers._2018.Services
{
    public class FileInstallationService
    {
        private FileInstallHandler InstallHandler { get; set; }

        public FileInstallationService()
        {
            Initialize();
        }

        private void Initialize()
        {
            InstallHandler = new FileInstallHandler();
        }

        public void InstallOnFileSystem(string appDir)
        {
            HandleRevit2018Installation(appDir);
            //ReplicateFilesToRevit2019Location();
        }

        public void HandleRevit2018Installation(string tempDir, string destDir = null)
        {
            var v2018 = destDir ?? RevitAddinLocationProvider.Revit2018AddinLocation;
            var finalDir = Path.Combine(v2018, "BRPLUSA");

            var files = Directory.EnumerateFiles(tempDir, "*", SearchOption.AllDirectories).ToArray();
            InstallHandler.HandleFileInstallation(files, finalDir);
        }

        public void CleanUpReplicationDirectory()
        {

        }
    }
}
