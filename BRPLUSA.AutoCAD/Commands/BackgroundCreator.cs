using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using BRPLUSA.AutoCAD.Extensions;
using BRPLUSA.AutoCAD.Services;
using BRPLUSA.AutoCAD.Wrappers;

namespace BRPLUSA.AutoCAD.Commands
{
    public class BackgroundCreator
    {
        private const string DrawingTemplate = @"G:\Shared\CADD\ACAD2011\Template\BR+A.dwt";
        public static Document CurrentDocument => Application.DocumentManager.MdiActiveDocument;
        public static Database CurrentDatabase => CurrentDocument.Database;
        public static string CurrentDirectory => Path.GetDirectoryName(CurrentDatabase.Filename);

        [CommandMethod("ProjSet", CommandFlags.Session)]
        public static void CreateBackgroundFromDrawing()
        {
            // Access and lock the current drawing document + database
            HostApplicationServices.WorkingDatabase = CADDatabaseUtilities.CurrentDatabase;

            /* If the drawing has any AEC or Proxy Objects, send out to
             * AECTOACAD, Audit, Purge and then start processing
             */

            ACADLayout[] layouts = { };

            // Create appropriate directories
            CADFileUtilities.CreateTypicalProjectDirectories();
            using (var locked = CurrentDocument.LockDocument())
            {
                // Gather a list of all necessary external references from document
                // Export each external reference to the file system
                CADDatabaseUtilities.ExportAllExternalReferences();

                // audit and purge the document of extraneous data
                CurrentDocument.CleanDocument();

                // Create a list of layouts in the document
                // Create a list of the viewports and their status/data
                layouts = CADDatabaseUtilities.CopyLayouts().ToArray();
            }

            // Save the consultant drawing to the file system
            SaveCurrentDrawing();

            // *************************************************

            // Create new drawing based on BR+A drawing template
            foreach (var layout in layouts)
            {
                var newDoc =
                    CurrentDocument.CreateNewDocument(DrawingTemplate, CADFileUtilities.NewBackgroundDirectory, layout.Name);

                Application.DocumentManager.MdiActiveDocument = newDoc;

                var newLayout = newDoc.CreateNewLayout("Work");

                newLayout.AddViewports(layout.Viewports);
            }

            // Insert consultant drawing as external reference

            // Create an appropiately sized layout based on Layout in Architectural Drawing

            // Place a BR+A Title Block and TBLKInfo on the Layout

            // audit and purge the document of extraneous data
            CurrentDocument.CleanDocument();
        }

        public static IEnumerable<Layout> GetAllLayoutsFromDocument()
        {
            return CADDatabaseUtilities.GetAllLayouts();
        }

        private static void SaveCurrentDrawing()
        {
            try
            {

                CurrentDatabase.SaveAs(CADFileUtilities.ReferenceBackgroundDirectory,
                    true,
                    DwgVersion.AC1800,
                    CurrentDatabase.SecurityParameters);
            }

            catch (Autodesk.AutoCAD.Runtime.Exception aex)
            {
                
            }
        }

        private static Document CreateNewDrawingFromTemplate(string drawingName)
        {
            var newDoc = CurrentDocument.CreateNewDocument(DrawingTemplate, CADFileUtilities.NewBackgroundDirectory,
                drawingName);

            return newDoc;
        }
    }
}
