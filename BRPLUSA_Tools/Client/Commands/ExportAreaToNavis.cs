using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Base;
using Document = Autodesk.Revit.DB.Document;

namespace BRPLUSA.Revit.Client.Commands
{
    public class ExportAreaToNavis : BaseCommand
    {
        protected override Result Work()
        {
            // draw box for area or pick a named scope box
            var view = Create3DViewFromArea();

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

            // export to NWC
            CurrentDocument.Export(projFolder, $"{projNumber}_ALL", opts);

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
            throw new NotImplementedException();
        }
    }
}
