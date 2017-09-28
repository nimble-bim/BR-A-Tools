﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Exception = System.Exception;

namespace BRPLUSA.AutoCAD.Services
{
    public static class CADDatabaseUtilities
    {
        private static Document CurrentDocument => Application.DocumentManager.MdiActiveDocument;
        private static Database CurrentDatabase => CurrentDocument.Database;
        private static string CurrentDirectory => Path.GetDirectoryName(CurrentDatabase.Filename);

        public static IEnumerable<BlockTableRecord> GetAllBlockTableRecords()
        {
            BlockTableRecord[] records;

            using (var tr = CurrentDatabase.TransactionManager.StartTransaction())
            {
                var blockTable = (BlockTable) tr.GetObject(CurrentDatabase.BlockTableId, OpenMode.ForRead);

                var ids = blockTable.Cast<ObjectId>();

                records = ids.Select(id => (BlockTableRecord) tr.GetObject(id, OpenMode.ForRead)).ToArray();
            }

            return records;
        }

        public static IEnumerable<Layout> GetAllLayouts()
        {
            var records = GetAllBlockTableRecords().Where(r => r.IsLayout);
            Layout[] layouts;

            using (var tr = CurrentDatabase.TransactionManager.StartTransaction())
            {
                layouts = records.Select(r => (Layout) tr.GetObject(r.Id, OpenMode.ForRead)).ToArray();
            }

            return layouts;
        }

        /// <summary>
        /// Gets a collection of the BlockTableRecords that are actually External References
        /// </summary>
        /// <returns>A collection of BlockTableRecords</returns>
        public static IEnumerable<BlockTableRecord> GetAllExternalReferences()
        {
            BlockTableRecord[] xrefs;

            using (var tr = CurrentDocument.TransactionManager.StartTransaction())
            {
                xrefs = GetAllBlockTableRecords().Where(record => record.IsFromExternalReference).ToArray();
                tr.Commit();
            }

            return xrefs;
        }

        public static void CopyExternalReference(BlockTableRecord xref, string pathToCopyTo)
        {
            try
            {
                var xrefFile = FindExternalReference(xref);

                File.Copy(xrefFile, pathToCopyTo, true);
            }

            catch (Exception e)
            {
                
            }
        }

        private static string FindExternalReference(BlockTableRecord xref)
        {
            try
            {
                var drawings = FileUtilities.GetAllDrawingFiles(CurrentDirectory);

                var xrefFile = drawings.FirstOrDefault(d => Path.GetFileName(d).Equals(xref.Name + ".dwg"));

                return xrefFile;
            }

            catch(Exception e)
            {
                throw e;
            }
        }

        public static void ExportExternalReference(BlockTableRecord xref, string pathToExportTo)
        {
            try
            {
                var xrefResolved = CheckExternalReferenceResolution(xref);

                if (!xrefResolved)
                    throw new Autodesk.AutoCAD.Runtime.Exception();

                using (var tr = CurrentDatabase.TransactionManager.StartTransaction())
                {
                    using (var xrefDb = xref.GetXrefDatabase(true))
                    {
                        CurrentDatabase.WblockCloneObjects(
                            new ObjectIdCollection(),
                            xref.Id,
                            new IdMapping(),
                            DuplicateRecordCloning.Ignore,
                            false);

                        tr.Commit();

                        SaveExternalReferenceToFileSystem(xrefDb, pathToExportTo);
                    }
                }
            }

            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                if(e.ErrorStatus == ErrorStatus.InvalidKey)
                    CurrentDocument.Editor.WriteMessage("\n" + $"{xref.Name} was not found, please ask the client to send it");
            }

            catch (Exception e)
            {
                
            }
        }

        private static void SaveExternalReferenceToFileSystem(Database xrefDb, string pathToExportTo)
        {
            if (!Directory.Exists(pathToExportTo))
                Directory.CreateDirectory(Path.GetDirectoryName(pathToExportTo) ?? throw new InvalidOperationException("The requested file path does not exist"));

            if (!Path.HasExtension(pathToExportTo))
                pathToExportTo += ".dwg";

            xrefDb.SaveAs(pathToExportTo, DwgVersion.AC1021);
        }

        private static bool CheckExternalReferenceResolution(BlockTableRecord xref)
        {
            if (!xref.IsResolved)
                CurrentDatabase.ResolveXrefs(true, false);

            if (!xref.IsResolved)
                ForceExternalReferenceResolution(xref);

            return xref.IsResolved;
        }

        private static void ForceExternalReferenceResolution(BlockTableRecord xref)
        {
            var xrefFile = FindExternalReference(xref);

            ModifyExternalReferenceSavedPath(xref, xrefFile);
        }

        private static bool ModifyExternalReferenceSavedPath(BlockTableRecord xref, string newPath)
        {
            return ModifyBlockTableRecordField(xref, "PatName", newPath);
        }

        private static bool ModifyBlockTableRecordField(BlockTableRecord obj, string fieldName, string fieldValue)
        {
            using (var tr = CurrentDatabase.TransactionManager.StartTransaction())
            {
                if (!obj.HasFields)
                    return false;

                var fields = ((Field) tr.GetObject(obj.GetField(), OpenMode.ForRead)).GetChildren();

                var requestedField = fields.FirstOrDefault(f => f.Equals(fieldName));

                var fieldCode = requestedField.GetFieldCode(FieldCodeFlags.AddMarkers | FieldCodeFlags.FieldCode);

                var newField = new Field(fieldValue);

                obj.SetField(fieldCode, newField);

                tr.Commit();
            }

            return obj.FieldValueEquals(fieldName, fieldValue);
        }

        private static bool FieldValueEquals(this BlockTableRecord record, string fieldName, string fieldValue)
        {
            throw new NotImplementedException();
        }
    }
}