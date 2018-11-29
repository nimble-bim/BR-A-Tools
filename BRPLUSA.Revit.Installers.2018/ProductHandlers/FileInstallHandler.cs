using System;
using System.Collections.Generic;
using System.IO;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Installers._2018.Services;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class FileInstallHandler
    {
        private Dictionary<string, string> FileLibrary { get; set; }

        public FileInstallHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            FileLibrary = new Dictionary<string, string>();
        }

        private bool IsAddin(string filename)
        {
            return filename.EndsWith(".addin");
        }

        private bool IsDebugFile(string filename)
        {
            return filename.ToLower().Contains("debug") || filename.EndsWith(".pdb");
        }

        public void HandleFileInstallation(IEnumerable<string> files, string installDir)
        {
            try
            {
                AddFilesToInstallationList(files, installDir);
                InstallFilesOnSystem(FileLibrary);
            }

            catch (Exception e)
            {
                LoggingService.LogError("Issues copying files to the system", e);
            }
        }

        public void AddFilesToInstallationList(IEnumerable<string> files, string installDir)
        {
            foreach (var file in files)
            {
                if (IsDebugFile(file))
                    continue;

                if (IsAddin(file))
                {
                    PushAddinFileToLibrary(file, installDir);
                    continue;
                }

                PushOtherFilesToLibrary(file, installDir);
            }
        }

        public void PushAddinFileToLibrary(string origin, string destination)
        {
            var filename = Path.GetFileName(origin);
            var destFilename = Path.Combine(destination, filename);

            FileLibrary.Add(origin, destFilename);
        }

        public void PushOtherFilesToLibrary(string origin, string destination)
        {
            var destFilename = ProductInstallationUtilities.FinalizeInstallLocation(origin, destination);

            FileLibrary.Add(origin, destFilename);
        }

        public void InstallFilesOnSystem(Dictionary<string, string> pool)
        {
            foreach (var file in pool)
            {
                InstallFileOnSystem(file.Key, file.Value);
            }
        }

        public void InstallFileOnSystem(string origin, string destination)
        {
            try
            {
                Directory.CreateDirectory(Directory.GetParent(destination).ToString());
                File.Copy(origin, destination, true);
            }

            catch (Exception e)
            {
                LoggingService.LogError($"Issue installing {origin} on system here: {destination}", e);
            }
        }
    }
}