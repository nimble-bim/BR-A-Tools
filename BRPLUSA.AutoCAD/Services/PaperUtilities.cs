using System;
using System.Linq;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.AutoCAD.PaperSizes;
using BRPLUSA.AutoCAD.Wrappers;

namespace BRPLUSA.AutoCAD.Services
{
    public static class PaperUtilities
    {
        public static double[] CommonPaperValues = { 8.5, 11, 17, 22, 24, 30, 34, 36, 42, 48 };
        private const double Tolerance = .5;

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

        public static PaperSize CalculatePaperSize(double xMin, double xMax, double yMin, double yMax)
        {
            var roughSize = CalculateRoughPaperSize(xMin, xMax, yMin, yMax);
            
            var common = CommonPaperSizes.FirstOrDefault(c => 
                            c.SizeValue == roughSize || 
                            c.SizeValue.Reverse() == roughSize);

            if (common != null)
                return common;

            // 


        }

        private static double[] CalculateRoughPaperSize(double xMin, double xMax, double yMin, double yMax)
        {
            double[] coords = { xMin, xMax, yMin, yMax };

            var xTotal = Math.Round(coords[0] - coords[1]);
            var yTotal = Math.Round(coords[2] - coords[3]);

            var xRound = RoundRoughSizeToCommonSize(xTotal);
            var yRound = RoundRoughSizeToCommonSize(yTotal);

            return new[] { xRound, yRound };
        }

        // Finds the size closest to the more common paper values
        private static double RoundRoughSizeToCommonSize(double size)
        {
            if (CommonPaperValues.Contains(size))
                return size;

            foreach (var value in CommonPaperValues)
            {
                if (value >= size - Tolerance)
                    return value;
            }

            return CommonPaperValues[CommonPaperValues.Length];

        }

        public static PaperOrientation CalculatePaperOrientation(Layout layout)
        {
            throw new NotImplementedException();
        }

    }
}
