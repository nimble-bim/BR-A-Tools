using System.IO;
using BRPLUSA.Revit.Installers._2018.Providers;

namespace BRPLUSA.Revit.Installers._2018.Services
{
    public class FileReplicationService
    {
        public void ReplicateFilesToRevitLocations(string appDir)
        {
            ReplicateFilesToRevit2018AddinLocation(appDir);
            //ReplicateFilesToRevit2019Location();
        }

        public void ReplicateFilesToRevit2018AddinLocation(string appDir, string finalDir = null)
        {
            var v2018 = finalDir ?? RevitAddinLocationProvider.Revit2018AddinLocation;

            var files = Directory.EnumerateFiles(appDir, "*", SearchOption.AllDirectories);

            var file = File.Create($@"{v2018}\test.txt");
            file?.Close();

            //foreach(var file in files)
            //{
            //    var name = Path.GetFileName(file);

            //    File.Copy(file, $@"{v2018}\BRPLUSA\{name}");
            //}
        }

        public void CleanUpReplicationDirectory()
        {

        }
    }
}
