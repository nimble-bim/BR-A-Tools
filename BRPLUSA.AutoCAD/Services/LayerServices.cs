using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.AutoCAD.Extensions;

namespace BRPLUSA.AutoCAD.Services
{
    public static class LayerServices
    {
        /// <summary>
        /// Tries to get the layer by name and creates it if it doesn't exist
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        private static LayerTableRecord GetLayer(this Document doc, string layerName, bool isLocked = false, bool isFrozen = false)
        {
            try
            {
                var db = doc.Database;
                var record = new LayerTableRecord();

                using (var tr = db.TransactionManager.StartTransaction())
                {
                    var table = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                    record = (LayerTableRecord)tr.GetObject(table[layerName], OpenMode.ForWrite, false, true);
                    record.IsFrozen = isFrozen;
                    record.IsLocked = isLocked;

                    tr.Commit();
                }

                return record;
            }

            catch (Exception e)
            {
                return doc.CreateNewLayer(layerName, isLocked, isFrozen);
            }
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
                    table = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
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
