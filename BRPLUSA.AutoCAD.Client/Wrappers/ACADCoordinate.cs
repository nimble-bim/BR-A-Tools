namespace BRPLUSA.Client.AutoCAD.Wrappers
{
    public class ACADCoordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public ACADCoordinate() { }

        public ACADCoordinate(double x, double y, double z = 0.0)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
