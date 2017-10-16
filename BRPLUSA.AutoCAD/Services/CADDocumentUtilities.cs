using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.AutoCAD.Extensions;
using BRPLUSA.AutoCAD.Mappers;
using BRPLUSA.AutoCAD.Wrappers;

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

        public static void CleanDocument(this Document doc)
        {
            doc.ForceDocumentAudit();
            doc.ForceDocumentPurge();
        }

        private static void ForceDocumentAudit(this Document doc)
        {
            try
            {
                doc.SendStringToExecute("_AUDIT\n", true, false, true);
                doc.SendStringToExecute("Y\n", true, false, true);
            }

            catch
            {
            }
        }

        private static void ForceDocumentPurge(this Document doc)
        {
            try
            {
                doc.SendStringToExecute("-PURGE\n", true, false, true);
                doc.SendStringToExecute("All\n", true, false, true);
                doc.SendStringToExecute("*\n", true, false, true);
                doc.SendStringToExecute("N\n", true, false, true);
            }

            catch
            {
            }
        }

        public static IEnumerable<ACADLayout> CopyLayouts()
        {
            using (var tr = CurrentDatabase.TransactionManager.StartTransaction())
            {
                var layouts = CADDatabaseUtilities.GetAllLayouts();



                tr.Commit();
            }
        }

        public static IEnumerable<Viewport> GetAllViewports(Layout layout)
        {
            ObjectId[] vpIds = {};
            layout.GetViewports().CopyTo(vpIds, 0);

            var viewports = vpIds.Select(v => (Viewport) v.GetObject(OpenMode.ForRead, false, true));

            return viewports;
        }

        private static ACADLayout MapLayout(Layout layout)
        {
            return new LayoutMapper().Map(layout);
        }

        public static ACADViewport MapViewport(Viewport viewport)
        {
            return new ViewportMapper().Map(viewport);
        }
    }
}
