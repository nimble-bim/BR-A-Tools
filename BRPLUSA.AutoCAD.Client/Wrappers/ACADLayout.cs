using System.Collections.Generic;
using BRPLUSA.Client.AutoCAD.PaperSizes;

namespace BRPLUSA.Client.AutoCAD.Wrappers
{
    public class ACADLayout
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public PaperSize PaperSize { get; set; }
        public PaperOrientation Orientation { get; set; }
        public IEnumerable<ACADViewport> Viewports { get; set; }
        public string Name { get; set; }

        public ACADLayout()
        {
            Initialize();
        }

        private void Initialize()
        {
            //throw new NotImplementedException();
        }
    }
}
