using System;
using System.IO;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

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

        /// <summary>
        /// An extension method which adds an external reference to the current document
        /// </summary>
        /// <param name="doc">The document which should have the xref added to it</param>
        /// <param name="xrefPath">The path of the xref to be added</param>
        /// <param name="xrefName">How the name of the xref should appear in the new document</param>
        /// <param name="layoutName">Space where the xref should be added; can only be Model or PaperSpace</param>
        public static void AttachExternalReference(this Document doc, string xrefPath, string xrefName, string layoutName)
        {
            
        }

        private static void CreateNewLayer(this Document doc, string layerName, bool isLocked = false, bool isFrozen = false)
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
        }

        private static LayerTableRecord GetLayer(this Document doc, string layerName)
        {
            var db = doc.Database;
            var record = new LayerTableRecord();

            using (var tr = db.TransactionManager.StartTransaction())
            {
                var table = (LayerTable) tr.GetObject(db.LayerTableId, OpenMode.ForRead);

                record.Name = layerName;

                table.UpgradeOpen();
                table.Add(record);

                tr.AddNewlyCreatedDBObject(record, true);

                tr.Commit();
            }

            return record;
        }

        private static void LockLayer(this Document doc, string layerName)
        {
            var layer = doc.GetLayer(layerName);

            doc.LockLayer(layer);
        }

        private static void LockLayer(this Document doc, LayerTableRecord layer)
        {
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                layer.IsLocked = true;
                tr.Commit();
            }
        }

        private static void UnlockLayer(this Document doc, string layerName)
        {
            var layer = doc.GetLayer(layerName);

            doc.UnlockLayer(layer);
        }

        private static void UnlockLayer(this Document doc, LayerTableRecord layer)
        {
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                layer.IsLocked = false;
                tr.Commit();
            }
        }

        private static void FreezeLayer(this Document doc, LayerTableRecord layer)
        {
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                layer.IsFrozen = true;
                tr.Commit();
            }
        }

        private static void ThawLayer(this Document doc, LayerTableRecord layer)
        {
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                layer.IsFrozen = false;
                tr.Commit();
            }
        }

        private static LayerTable GetLayerTable(this Document doc)
        {
            var db = doc.Database;
            try
            {
                LayerTable table;

                using (var tr = db.TransactionManager.StartTransaction())
                {
                    table = (LayerTable) tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                    tr.Commit();
                }

                return table;
            }

            catch
            {
                throw new Exception();
            }
        }
    }
}
