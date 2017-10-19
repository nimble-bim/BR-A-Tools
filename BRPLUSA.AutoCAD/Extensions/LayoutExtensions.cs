using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using BRPLUSA.AutoCAD.Services;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

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

        public static void SetPageSize(this Layout layout)
        {
            using (var docLock = CurrentDocument.LockDocument())
            {
                using (var tr = CurrentDatabase.TransactionManager.StartTransaction())
                {
                    using (var settings = new PlotSettings(layout.ModelType))
                    {
                        settings.CopyFrom(layout);
                        var validator = PlotSettingsValidator.Current;
                        validator.SetZoomToPaperOnUpdate(settings, true);

                        var newLayout = (Layout)tr.GetObject(layout.ObjectId, OpenMode.ForRead);
                        newLayout.UpgradeOpen();
                        newLayout.CopyFrom(settings);

                        tr.Commit();
                    }
                }
            }
        }

        public static IEnumerable<Viewport> GetAllViewports(this Layout layout)
        {
            var viewports = new Viewport[] { };

            using (var tr = CurrentDatabase.TransactionManager.StartTransaction())
            {
                if (layout.ModelType)
                    return null;

                var viewportIds = layout.GetViewports().Cast<ObjectId>();

                viewports = viewportIds.Select(CADDatabaseUtilities.TryObjectConversionFromObjectId<Viewport>).ToArray();
            }

            return viewports;
        }

        public static Layout CreateNewLayout(this Document doc, string name)
        {
            var layout = new Layout();

            using (var locked = doc.LockDocument())
            {
                using (var tr = doc.Database.TransactionManager.StartTransaction())
                {
                    var manager = LayoutManager.Current;
                    var id = new ObjectId();

                    try
                    {
                        id = manager.CreateLayout(name);
                    }

                    catch(Exception e)
                    {
                        if (e.ErrorStatus == ErrorStatus.DuplicateKey)
                            id = manager.CreateLayout(name + "(1)");
                    }

                    layout = (Layout) tr.GetObject(id, OpenMode.ForWrite);
                }

                layout.RemoveAllViewports();
            }

            return layout;
        }

        public static void RemoveAllViewports(this Layout layout)
        {
            var db = layout.Database;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                var layoutRecord = (BlockTableRecord) tr.GetObject(layout.BlockTableRecordId, OpenMode.ForWrite);

                var viewportType = RXObject.GetClass(typeof(Viewport));

                foreach (var obj in layoutRecord)
                {
                    if(obj.ObjectClass != viewportType)
                        continue;

                    var vp = (Viewport) tr.GetObject(obj, OpenMode.ForWrite);
                    vp.Erase();

                    CurrentDocument.Editor.Regen();
                }

                tr.Commit();
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
