using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BRPLUSA.Navisworks;
using BRPLUSA.Revit.Base;
using BRPLUSA.Revit.Exceptions;
using Document = Autodesk.Revit.DB.Document;

namespace BRPLUSA.Revit.Client.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExportAreaToNavis : BaseCommand
    {
        protected override Result Work()
        {
            // draw box for area or pick a named scope box
            var view = Create3DViewFromArea();

            if(view == null)
                throw new CancellableException();

            // create options for export
            var opts = new NavisworksExportOptions
            {
                ConvertElementProperties = true,
                Coordinates = NavisworksCoordinates.Shared,
                ExportLinks = true,
                ExportScope = NavisworksExportScope.View,
                ViewId = view.Id
            };

            // if the export of a specific VIEW for each linked model doesn't work - 
            // open other linked Revit models and create the same 3D for each

            // create folders and other variables
            var projFolder = FindProjectFolder(CurrentDocument);
            var projNumber = GetProjectNumber(projFolder);
            var navisFileName = $"{projNumber}_ALL";

            // export to NWC
            CurrentDocument.Export(projFolder, navisFileName, opts);

            // starts clash detection in separate thread
            StartClashDetectionThread(navisFileName);

            return Result.Succeeded;

            // switch over to Navisworks context

            /* 
             * Append all files
             * import Search Sets
             * import Clash Tests
             * Run Clash Tests
             * Split based on typical responsibility
             * Export to HTML
             * Export to Viewpoints
             */
        }

        private void StartClashDetectionThread(string navisFileName)
        {
            void ClashThread()
            {
                NavisworksServer.InitiateClashDetection(navisFileName);
            }

            var thread = new Thread(ClashThread);
            thread.Start();
        }

        private string GetProjectNumber(string projFolder)
        {
            throw new NotImplementedException();
        }

        private string FindProjectFolder(Document currentDocument)
        {
            throw new NotImplementedException();
        }

        private View Create3DViewFromArea()
        {
            View currentView = CurrentDocument.ActiveView;
            View3D finalView = null;
            PickedBox area = null;
            BoundingBoxXYZ viewBox = null;

            // Check that the user is in a plan (for now, allow changes to use Section space later)
            if (currentView.ViewType != ViewType.CeilingPlan && currentView.ViewType != ViewType.FloorPlan)
            {
                TaskDialog.Show("Error", "This command can only be used in plan view at the moment");
                throw new CancellableException();
            }

            //// Prompt the user to select a bounding box
            //using (Selection userBox = UiDocument.Selection)
            //{
            //    area = userBox.PickBox(PickBoxStyle.Crossing, "Please draw the area you'd like to see in 3D");
            //}

            // Create a new 3D Area from this bound box as well as the Plan's section box
            var view3Ds = new FilteredElementCollector(CurrentDocument).OfClass(typeof(ViewFamilyType));

            var view3DId = view3Ds
                .Cast<ViewFamilyType>()
                .FirstOrDefault(v => v.ViewFamily == ViewFamily.ThreeDimensional).Id;

            using (var sub = new SubTransaction(CurrentDocument))
            {
                if(!sub.HasStarted())
                    sub.Start();

                // Create new 3D View
                finalView = View3D.CreateIsometric(CurrentDocument, view3DId);
                finalView.IsSectionBoxActive = true;

                try
                {

                    var box = new BoundingBoxXYZ
                    {
                        Enabled = true,
                        Min = new XYZ(-100.0, -100.0, -100.0),
                        Max = new XYZ(100.0, 100.0, 100.0)
                    };

                    finalView.SetSectionBox(box);
                }

                catch (ArgumentException args)
                {
                    Console.WriteLine($"Something happened: \n{args}");
                    sub.RollBack();
                    throw new CancellableException();
                }

                sub.Commit();
            }

            // change to the newly created view
            //UiDocument.RequestViewChange(finalView);

            return finalView;
        }

        
    }
}
