using System;
using System.IO;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using BRPLUSA.AutoCAD.Services;

namespace BRPLUSA.AutoCAD.Extensions
{
    public static class DocumentExtensions
    {
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


        public static Document CreateNewDocument(this Document doc, string templateLocation, string directoryToSaveIn, string dwgName)
        {
            var docmgr = Application.DocumentManager;
            var newdoc = docmgr.Add(templateLocation);

            //switch the active drawing to the newly made drawing
            docmgr.MdiActiveDocument = newdoc;
            var fileName = "Setup_" + Path.GetFileNameWithoutExtension(doc.Name) + "_" + dwgName + ".dwg";

            newdoc.Database.SaveAs(
                Path.Combine(directoryToSaveIn, fileName),
                true,
                DwgVersion.AC1800,
                doc.Database.SecurityParameters);

            return newdoc;
        }

        public static LayerTableRecord CreateNewLayer(this Document doc, string layerName, bool isLocked = false, bool isFrozen = false)
        {
            var db = doc.Database;
            var record = new LayerTableRecord
            {
                Name = layerName,
                IsLocked = isLocked,
                IsFrozen = isFrozen
            };

            using (var tr = db.TransactionManager.StartTransaction())
            {
                var table = (LayerTable) tr.GetObject(db.LayerTableId, OpenMode.ForRead);

                table.UpgradeOpen();
                tr.AddNewlyCreatedDBObject(record, true);

                tr.Commit();
            }

            return record;
        }
    }
}
