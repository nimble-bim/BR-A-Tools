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
        private readonly ViewportMapper _vpMapper = new ViewportMapper();

        public ACADLayout Map(Layout layout)
        {
            var xMin = layout.Limits.MinPoint.X;
            var xMax = layout.Limits.MaxPoint.Y;
            var yMin = layout.Limits.MinPoint.X;
            var yMax = layout.Limits.MaxPoint.Y;

            var viewports = CADDatabaseUtilities.GetAllViewports(layout);

            return new ACADLayout
            {
                Name = layout.LayoutName,
                Height = Math.Round(layout.PlotPaperSize.X / 25.4),
                Width = Math.Round(layout.PlotPaperSize.Y / 25.4),
                PaperSize = PaperUtilities.CalculatePaperSize(xMin, xMax, yMin, yMax),
                Orientation = PaperUtilities.CalculatePaperOrientation(xMin + xMax, yMin + yMax ),
                Viewports = viewports.Select(vp => _vpMapper.Map(vp))
            };
        }
    }
}
