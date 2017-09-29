using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.AutoCAD.Extensions;

namespace BRPLUSA.AutoCAD.Services
{
    public static class CADDocumentUtilities
    {
        private static Document CurrentDocument => Application.DocumentManager.MdiActiveDocument;
        private static Database CurrentDatabase => CurrentDocument.Database;
        private static string CurrentDirectory => Path.GetDirectoryName(CurrentDatabase.Filename);

        public static void RemoveAllEmptyLayouts()
        {
            using (var tr = CurrentDatabase.TransactionManager.StartTransaction())
            {
                var layouts = CADDatabaseUtilities.GetAllLayouts();

                if (!layouts.Any())
                    return;

                foreach (var layout in layouts)
                {
                    if(layout.IsEmpty())
                        LayoutManager.Current.DeleteLayout(layout.LayoutName);
                }

                tr.Commit();
            }
        }


    }
}
