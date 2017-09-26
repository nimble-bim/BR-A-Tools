namespace BRPLUSA.AutoCAD.PaperSizes
{
    public abstract class PaperSize
    {
        public abstract double X { get; }
        public abstract double Y { get; }

        public double[] SizeValue => new[] { X, Y };
    }
}
