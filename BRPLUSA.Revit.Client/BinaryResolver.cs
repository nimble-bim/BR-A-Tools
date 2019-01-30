using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Revit.Client.EndUser.Applications;

namespace BRPLUSA.Revit.Client
{
    public static class BinaryResolver
    {
        public static void ResolveOwnBinaries()
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    if (!args.Name.StartsWith("BRPLUSA"))
                        return null;

                    string assName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                    var location = Path.GetDirectoryName(typeof(RevitApplicationEnhancements).Assembly.Location);
                    var path = Path.Combine(location, assName);

                    return File.Exists(path)
                        ? Assembly.LoadFile(path)
                        : null;
                };
            }

            catch (Exception e)
            {
                var ex = new Exception("Fatal error! Failed to resolve UI binaries", e);

                throw ex;
            }
        }

        public static void ResolveBrowserBinaries()
        {
            //try
            //{
            //    LoggingService.LogInfo("Attempting to resolve browser binaries");

            //    AppDomain.CurrentDomain.AssemblyResolve += BardWebClient.ResolveCefBinaries;
            //    BardWebClient.InitializeCefSharp();

            //    LoggingService.LogInfo("Browser binary resolution complete");
            //}

            //catch (Exception e)
            //{
            //    var ex = new Exception("Fatal error! Failed to resolve browser binaries", e);

            //    throw ex;
            //}
        }

        public static void ResolveUIBinaries()
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    if (!args.Name.StartsWith("Dragablz") && !args.Name.StartsWith("MaterialDesign"))
                        return null;

                    string assName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                    var location = Path.GetDirectoryName(typeof(RevitApplicationEnhancements).Assembly.Location);
                    var path = Path.Combine(location, assName);

                    return File.Exists(path)
                        ? Assembly.LoadFile(path)
                        : null;
                };
            }

            catch (Exception e)
            {
                var ex = new Exception("Fatal error! Failed to resolve UI binaries", e);

                throw ex;
            }
        }
    }
}
