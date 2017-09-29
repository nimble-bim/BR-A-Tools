using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;

namespace BRPLUSA.AutoCAD
{
    public class BackgroundCreator
    {
        [CommandMethod("ProjSet", CommandFlags.Session)]
        public static void CreateBackgroundFromDrawing()
        {
            // Access and lock the current drawing document + database

            /* If the drawing has any AEC or Proxy Objects, send out to
             * AECTOACAD, Audit, Purge and then start processing
             */

            // Create appropriate directories

            // Gather a list of all necessary external references from document

            // Export each external reference to the file system

            // Create a list of layouts in the document

            // Create a list of the viewports and their status/data

            // Save the consultant drawing to the file system

            // *************************************************

            // Create new drawing based on BR+A drawing template

            // Insert consultant drawing as external reference

            // Create an appropiately sized layout based on Layout in Architectural Drawing

            // Place a BR+A Title Block and TBLKInfo on the Layout
        }
    }
}
