using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.AutoCAD.Wrappers;

namespace BRPLUSA.AutoCAD.Mappers
{
    public class LayoutMapper : IMapper<Layout, ACADLayout>
    {
        public ACADLayout Map(Layout layout)
        {
            return new ACADLayout
            {
                Height = Math.Round(layout.PlotPaperSize.X / 25.4),
                Width = Math.Round(layout.PlotPaperSize.Y / 25.4),
                PaperSize = PaperUtilities.CalculatePaperSize(layout),
                Orientation = PaperUtilities.CalculatePaperOrientation(layout)
            };
        }
    }
}
