using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.Core;

namespace BRPLUSA.AutoCAD.Services
{
    public static class CADFileUtilities
    {
        private const string XrefFolderName = "SETUP";
        private const string DwgFolderName = "_Setup Files";

        /// <summary>
        /// Recursively gathers all drawing files
        /// 
        /// Drawing files are any with an .dwg file extension
        /// </summary>
        /// <param name="path">file to start search from</param>
        public static IEnumerable<string> GetAllDrawingFiles(string path)
        {
            return FileUtility.GetAllFilesOfType(path, "*.dwg");
        }

        public static void CreateTypicalProjectDirectories()
        {
            var docManager = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager;
            var doc = docManager.MdiActiveDocument;
            var db = doc.Database;
            HostApplicationServices.WorkingDatabase = db;

            var currentLocation = db.Filename;

            var xrefLoc = Path.Combine(currentLocation, XrefFolderName);
            var newDwgLoc = Path.Combine(xrefLoc, DwgFolderName);

            FileUtility.CreateDirectory(newDwgLoc);
        }
    }
}
