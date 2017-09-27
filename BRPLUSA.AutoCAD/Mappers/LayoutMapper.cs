using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.AutoCAD.Services;
using BRPLUSA.AutoCAD.Wrappers;

namespace BRPLUSA.AutoCAD.Mappers
{
    public class LayoutMapper : IMapper<Layout, ACADLayout>
    {
        public ACADLayout Map(Layout layout)
        {
            var xMin = layout.Limits.MinPoint.X;
            var xMax = layout.Limits.MaxPoint.Y;
            var yMin = layout.Limits.MinPoint.X;
            var yMax = layout.Limits.MaxPoint.Y;

            return new ACADLayout
            {
                Height = Math.Round(layout.PlotPaperSize.X / 25.4),
                Width = Math.Round(layout.PlotPaperSize.Y / 25.4),
                PaperSize = PaperUtilities.CalculatePaperSize(xMin, xMax, yMin, yMax),
                Orientation = PaperUtilities.CalculatePaperOrientation(layout)
            };
        }
    }
}
