using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.AutoCAD.PaperSizes;
using BRPLUSA.AutoCAD.Wrappers;

namespace BRPLUSA.AutoCAD
{
    public static class PaperUtilities
    {
        public static double[] KnownPaperSizes = { 8.5, 11, 17, 22, 24, 30, 34, 36, 42, 48 };

        private static PaperSize CalculatePaperSize(Layout layout)
        {
            var xMin = layout.Limits.MinPoint.X;
            var xMax = layout.Limits.MaxPoint.Y;
            var yMin = layout.Limits.MinPoint.X;
            var yMax = layout.Limits.MaxPoint.Y;

            double[] coords = { xMin, xMax, yMin, yMax };

            var xTotal = Math.Round(coords[0] - coords[1]);
            var yTotal = Math.Round(coords[2] - coords[3]);
        }

        private static PaperOrientation CalculatePaperOrientation(Layout layout)
        {
            throw new NotImplementedException();
        }

    }
}
