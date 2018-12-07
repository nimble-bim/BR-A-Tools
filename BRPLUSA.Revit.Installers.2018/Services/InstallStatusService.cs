using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018.Entities;
using BRPLUSA.Revit.Installers._2018.Providers;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.Services
{
    public class InstallStatusService
    {
        public static bool CheckIfRevit2018Installed()
        {
            LoggingService.LogInfo("Checking if Revit 2018 is installed");
            var installed = RevitAddinLocationProvider.IsRevitVersionInstalled(RevitVersion.V2018);
            LoggingService.LogInfo(installed ? "Revit 2018 is installed" : "Revit 2018 is not installed");

            return installed;
        }

        public static bool CheckIf2018AppIsInstalled()
        {
            return CheckIf2018AppIsInstalledAsync().Result;
        }

        public static async Task<bool> CheckIf2018AppIsInstalledAsync()
        {
            LoggingService.LogInfo("Checking if our app for Revit 2018 is installed");

            var v2018 = RevitAddinLocationProvider.GetRevitAddinFolderLocation(RevitVersion.V2018);
            var addinFiles = await Task.Run(() => Directory.EnumerateFiles(v2018).ToArray());
            var installed = addinFiles.Any(fileName => fileName.Contains("BRPLUSA.addin"));

            LoggingService.LogInfo(installed ? "Our app is installed" : "Our app is not installed");

            return installed;
        }

        public static bool CheckForUpdateTo2018App(UpdateManager mgr)
        {
            return CheckForUpdateTo2018AppAsync(mgr).Result;
        }

        public static async Task<bool> CheckForUpdateTo2018AppAsync(UpdateManager mgr)
        {
            try
            {
                var info = await GetVersionInformationFromServerAsync(mgr);
                var local = GetLocalVersion(info);
                var server = GetServerVersion(info);

                var update = local < server;

                LoggingService.LogInfo("Update check complete");
                LoggingService.LogInfo(update
                    ? "Update available"
                    : "No further updates available");

                return update;
            }

            catch (Exception e)
            {
                throw new Exception("Failed to retrieve information", e);
            }
        }

        public static async Task<UpdateInfo> GetVersionInformationFromServerAsync(UpdateManager mgr)
        {
            LoggingService.LogInfo("Checking for updated version of app software");
            var info = await mgr.CheckForUpdate();
            LoggingService.LogInfo("Check complete!");

            return info;
        }

        public static VersionData GetVersionData(ReleaseEntry info)
        {
            LoggingService.LogInfo("Getting version data...");
            var version = VersionService.GetVersionInformation(info);
            LoggingService.LogInfo($"Version is: {version}");

            return version;
        }

        public static VersionData GetLocalVersion(UpdateInfo info)
        {
            LoggingService.LogInfo("Confirming local version data...");
            var local = GetVersionData(info?.CurrentlyInstalledVersion);
            LoggingService.LogInfo("Local version confirmed");

            return local;
        }

        public static VersionData GetServerVersion(UpdateInfo info)
        {
            LoggingService.LogInfo("Confirming server version data...");
            var server = GetVersionData(info?.FutureReleaseEntry);
            LoggingService.LogInfo("Server version confirmed");

            return server;
        }
    }
}
