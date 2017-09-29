namespace BRPLUSA.AutoCAD.PaperSizes
{
    public class AnsiD : PaperSize
    {
        public override double X => 34;
        public override double Y => 22;
        public override string PlotConfigurationName { get; }
        public override string TitleBlockName { get; }
        public override string TitleBlockInformation { get; }
    }
}