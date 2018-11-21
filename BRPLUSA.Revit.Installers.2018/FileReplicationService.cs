using System.IO;

namespace BRPLUSA.Revit.Installers._2018
{
    public class FileReplicationService
    {
        public static void ReplicateFilesToRevit2018AddinLocation(string appDir, string finalDir = null)
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

        public static void CleanUpReplicationDirectory()
        {

        }
    }
}
