using System;
using System.IO;

namespace BRPLUSA.Revit.Installers._2018.Services
{
    public class ProductInstallationUtilities
    {
        public static string ReadVersionFromPath(string path)
        {
            if (!path.Contains("app"))
                throw new Exception("Error reading path - likely not an app installation path");

            var remainder = path.Substring(path.LastIndexOf("app-", StringComparison.Ordinal));
            var appVersion = remainder.Contains(@"\")
                ? remainder.Remove(remainder.IndexOf(@"\"))
                : remainder;

            var semVersion = appVersion.Substring(appVersion.LastIndexOf('-') + 1);

            return semVersion;
        }

        public static string FinalizeInstallLocation(string origin, string installDir)
        {
            var version = ReadVersionFromPath(origin);
            var fullVersionDir = $@"app-{version}";

            var pathWithParents = origin
                .Substring(origin.LastIndexOf(fullVersionDir, StringComparison.Ordinal))
                .TrimStart(fullVersionDir.ToCharArray());

            var destination = $@"{installDir}{pathWithParents}";

            return destination;
        }
    }
}