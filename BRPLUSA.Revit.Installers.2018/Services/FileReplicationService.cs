using System;
using System.IO;
using System.Linq;
using BRPLUSA.Core.Services;
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
            //TODO: check on this later because it won't grab the right version based on new updates
            var assemblyVersion = GetType().Assembly.GetName().Version.ToString();
            var semanticVersionNumber = assemblyVersion.Remove(assemblyVersion.LastIndexOf('.'));
            var fullVersion = $@"app-{semanticVersionNumber}";
            var packageDir = Path.Combine(appDir, $"{fullVersion}");

            var v2018 = finalDir ?? RevitAddinLocationProvider.Revit2018AddinLocation;
            var addinFolder = v2018;
            var appFolder = Path.Combine(v2018, "BRPLUSA");
            Directory.CreateDirectory(appFolder);

            var files = Directory.EnumerateFiles(packageDir, "*", SearchOption.AllDirectories).ToArray();

            foreach (var file in files)
            {
                
                var name = file
                    .Substring(file.LastIndexOf(fullVersion, StringComparison.Ordinal))
                    .TrimStart(fullVersion.ToCharArray());

                try
                {
                    if (name.EndsWith(".addin"))
                    {
                        name = Path.GetFileName(name);
                        var addinDestination = $@"{addinFolder}{name}";
                        //Directory.CreateDirectory(Directory.GetParent(addinDestination).ToString());
                        File.Copy(file, addinDestination, true);
                        continue;
                    }

                    var fileDestination = $@"{appFolder}{name}";
                    Directory.CreateDirectory(Directory.GetParent(fileDestination).ToString());
                    File.Copy(file, fileDestination, true);
                }

                catch (Exception e)
                {
                    LoggingService.LogError($"Error copying {name}. Continuing with remaining files", e);
                }
            }
        }

        public void CleanUpReplicationDirectory()
        {

        }
    }
}
