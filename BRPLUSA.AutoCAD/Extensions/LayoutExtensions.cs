using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.AutoCAD.Services;

namespace BRPLUSA.AutoCAD.Extensions
{
    public static class LayoutExtensions
    {
        private static Document CurrentDocument => Application.DocumentManager.MdiActiveDocument;
        private static Database CurrentDatabase => CurrentDocument.Database;
        private static string CurrentDirectory => Path.GetDirectoryName(CurrentDatabase.Filename);

        public static bool IsEmpty(this Layout layout)
        {
            using (var tr = CurrentDatabase.TransactionManager.StartTransaction())
            {
                var objId = layout.BlockTableRecordId;

                var record = (BlockTableRecord) tr.GetObject(objId, OpenMode.ForRead);

                tr.Commit();

                return !record.GetAllObjectsFromBlockTableRecord().Any();
            }
        }
    }

    public static class BlockTableRecordExtensions
    {
        public static IEnumerable<ObjectId> GetAllObjectsFromBlockTableRecord(this BlockTableRecord record)
        {
            var list = new List<ObjectId>();

            foreach (var id in record)
            {
                list.Add(id);
            }

            return list;
        }
    }
}
