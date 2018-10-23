using System.Collections.Generic;

namespace BRPLUSA.Client.AutoCAD.Wrappers
{
    public class ACADViewport
    {
        public double ViewHeight { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double CustomScale { get; set; }
        public IEnumerable<string> FrozenLayers { get; set; }
        public ACADCoordinate WindowMinimum { get; set; }
        public ACADCoordinate WindowMaximum { get; set; }
        public ACADCoordinate ViewTarget { get; set; }
        public ACADCoordinate ViewCenter { get; set; }
        public ACADCoordinate CenterPoint { get; set; }
        public int StandardScale { get; set; }
    }
}
