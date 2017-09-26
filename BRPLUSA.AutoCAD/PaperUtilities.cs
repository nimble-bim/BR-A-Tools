using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using BRPLUSA.AutoCAD.PaperSizes;
using BRPLUSA.AutoCAD.Wrappers;

namespace BRPLUSA.AutoCAD
{
    public static class PaperUtilities
    {
        public static double[] CommonPaperValues = { 8.5, 11, 17, 22, 24, 30, 34, 36, 42, 48 };

        public static PaperSize[] CommonPaperSizes => new PaperSize[]
        {
            new AnsiA(),
            new AnsiB(),
            new AnsiC(),
            new AnsiD(),
            new ArchC(),
            new ArchD(),
            new ArchE(),
            new ArchE1(),
        };

        public static PaperSize CalculatePaperSize(Layout layout)
        {
            var roughSize = CalculateRoughPaperSize(layout);

            var common = CommonPaperSizes.FirstOrDefault(c => c.SizeValue == roughSize || c.SizeValue.Reverse() == roughSize);

            if (common != null)
                return common;




        }

        private static double[] CalculateRoughPaperSize(Layout layout)
        {
            var xMin = layout.Limits.MinPoint.X;
            var xMax = layout.Limits.MaxPoint.Y;
            var yMin = layout.Limits.MinPoint.X;
            var yMax = layout.Limits.MaxPoint.Y;

            double[] coords = { xMin, xMax, yMin, yMax };

            var xTotal = Math.Round(coords[0] - coords[1]);
            var yTotal = Math.Round(coords[2] - coords[3]);

            return new[] { xTotal, yTotal };
        }

        private static double RoundRoughSize(double size)
        {
            if (CommonPaperValues.Contains(size))
                return size;

            foreach (var value in CommonPaperValues)
            {
                if (value >= size)
                    return value;
            }

            return CommonPaperValues[CommonPaperValues.Length];

        }

        private static PaperOrientation CalculatePaperOrientation(Layout layout)
        {
            throw new NotImplementedException();
        }

    }
}
