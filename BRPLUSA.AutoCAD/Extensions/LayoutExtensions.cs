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
using BRPLUSA.AutoCAD.Wrappers;
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

        public static void AddViewports(this Layout layout, IEnumerable<Viewport> viewports)
        {
            
        }

        public static void AddViewport(this Layout layout, Viewport viewport)
        {
            var db = layout.Database;
            var vportLayer = CurrentDocument.GetLayer("x-vport");

            using (var locked = CurrentDocument.LockDocument())
            {
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    //var table = (BlockTable) tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    var layoutRecord = (BlockTableRecord) tr.GetObject(layout.OwnerId, OpenMode.ForWrite);
                    
                    CurrentDocument.Editor.SwitchToPaperSpace();

                    var vp = new Viewport
                    {
                        Width = viewport.Width,
                        Height = viewport.Height,
                        CenterPoint = viewport.CenterPoint,
                    };

                    layoutRecord.AppendEntity(vp);
                    tr.AddNewlyCreatedDBObject(vp, true);

                    vp.ViewCenter = viewport.ViewCenter;
                    vp.ViewHeight = viewport.ViewHeight;
                    vp.ViewTarget = viewport.ViewTarget;
                    vp.FreezeLayersInViewport(viewport.Frozen);
                }
            }
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
