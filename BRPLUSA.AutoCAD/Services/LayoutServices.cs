using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using BRPLUSA.AutoCAD.Extensions;

namespace BRPLUSA.AutoCAD.Services
{
    public static class LayoutServices
    {
        public static void InsertTitleBlock(this Document doc)
        {
            
        }

        public static Layout CreateNewLayout(this Document doc, string layoutName)
        {
            var db = doc.Database;
            var newLayout = new Layout();

            using (var tr = db.TransactionManager.StartTransaction())
            {
                var manager = LayoutManager.Current;
                var newLayoutId = manager.CreateLayout(layoutName);
                newLayout = (Layout)tr.GetObject(newLayoutId, OpenMode.ForRead);

                tr.Commit();
            }

            return newLayout;
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
            if (File.Exists(xrefPath))
                return;

            var db = doc.Database;

            using (var locked = doc.LockDocument())
            {
                var xrefId = new ObjectId(IntPtr.Zero);
                doc.CreateNewLayer("x-xref");

                using (var tr = db.TransactionManager.StartTransaction())
                {
                    

                    try
                    {
                        xrefId = db.AttachXref(xrefPath, xrefName);
                    }

                    catch (Autodesk.AutoCAD.Runtime.Exception)
                    {

                    }



                }
            }
        }

        private static void PlaceExternalReferenceInModelSpace(this Document doc, ObjectId xrefId)
        {
            var db = doc.Database;

            using (var locked = doc.LockDocument())
            {
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    var origin = new Point3d(0, 0, 0);
                    var block = new BlockReference(origin, xrefId);

                    var table = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    var modelSpace = (BlockTableRecord)tr.GetObject(table[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                    modelSpace.AppendEntity(block);
                    tr.AddNewlyCreatedDBObject(block, true);

                    tr.Commit();
                }
            }
        }

        private static void PlaceExternalReferenceInLayout(this Document doc, ObjectId xrefId, string layoutName)
        {
            var db = doc.Database;
            var layout = doc.GetLayout(layoutName);

            using (var locked = doc.LockDocument())
            {
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    var origin = new Point3d(0, 0, 0);
                    var block = new BlockReference(origin, xrefId);

                    var layoutSpace = (BlockTableRecord)tr.GetObject(layout.OwnerId, OpenMode.ForRead);

                    layoutSpace.AppendEntity(block);
                    tr.AddNewlyCreatedDBObject(block, true);

                    tr.Commit();
                }
            }
        }


        /// <summary>
        /// Gets a named layout if existing or creates it if it doesn't
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="layoutName"></param>
        /// <returns>Returns a DBObject as a Layout</returns>
        public static Layout GetLayout(this Document doc, string layoutName)
        {
            try
            {
                var existing = CADDatabaseUtilities
                    .GetAllLayouts()
                    .Single(l => l.LayoutName.Equals(layoutName));

                return existing;
            }

            catch (Exception e)
            {
                return doc.CreateNewLayout(layoutName);
            }

        }

        private static Layout GetModelSpaceLayout(this Document doc)
        {
            var layouts = CADDatabaseUtilities.GetAllLayouts();

            var model = layouts.FirstOrDefault(lay => lay.LayoutName.ToUpper() == "MODEL");

            return model;
        }
    }
}
