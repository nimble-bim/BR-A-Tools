﻿using System;
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
