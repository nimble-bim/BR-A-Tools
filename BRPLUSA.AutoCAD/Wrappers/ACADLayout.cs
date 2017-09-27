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
        public double Height { get; set; }
        public double Width { get; set; }
        public PaperSize PaperSize { get; set; }
        public PaperOrientation Orientation { get; set; }
        public IEnumerable<ACADViewport> Viewports { get; set; }

        public ACADLayout()
        {
            Initialize();
        }

        private void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
