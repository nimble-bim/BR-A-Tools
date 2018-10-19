using System.Collections.Generic;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.Core;

namespace BRPLUSA.Client.AutoCAD.Services
{
    public static class CADFileUtilities
    {
        private const string XrefFolderName = "SETUP";
        private const string DwgFolderName = "_Setup Files";
        public static string ReferenceBackgroundDirectory { get; private set; }
        public static string NewBackgroundDirectory { get; private set; }

        /// <summary>
        /// Recursively gathers all drawing files
        /// 
        /// Drawing files are any with an .dwg file extension
        /// </summary>
        /// <param name="path">file to start search from</param>
        public static IEnumerable<string> GetAllDrawingFiles(string path)
        {
            return FileUtils.GetAllFilesOfType(path, "*.dwg");
        }

        public static void CreateTypicalProjectDirectories()
        {
            var docManager = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager;
            var doc = docManager.MdiActiveDocument;
            var db = doc.Database;
            HostApplicationServices.WorkingDatabase = db;

            var currentLocation = db.Filename;

            ReferenceBackgroundDirectory = Path.Combine(currentLocation, XrefFolderName);
            NewBackgroundDirectory = Path.Combine(ReferenceBackgroundDirectory, DwgFolderName);

            FileUtils.CreateDirectory(NewBackgroundDirectory);
        }
    }
}
