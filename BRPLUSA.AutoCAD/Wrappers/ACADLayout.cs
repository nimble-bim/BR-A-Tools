using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.AutoCAD.PaperSizes;

namespace BRPLUSA.AutoCAD.Wrappers
{
    public class ACADLayout
    {
        private readonly Layout _layout;

        public double Height => Math.Round(_layout.PlotPaperSize.X / 25.4 );

        public double Width => Math.Round(_layout.PlotPaperSize.Y / 25.4);

        public PaperSize PaperSize { get; private set; }
        public PaperOrientation Orientation { get; private set; }

        public ACADLayout(Layout lay)
        {
            _layout = lay;
            Initialize();
        }

        private void Initialize()
        {
            CalculatePaperSize();
            CalculateOrientation();
        }

        private void CalculatePaperSize()
        {
            PaperSize = PaperUtilities.CalculatePaperSize(_layout);
        }

        private void CalculateOrientation()
        {
            Orientation = PaperUtilities.CalculatePaperOrientation(_layout);
        }
    }
}
