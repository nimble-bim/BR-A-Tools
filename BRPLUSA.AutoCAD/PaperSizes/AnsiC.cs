namespace BRPLUSA.AutoCAD.PaperSizes
{
    public class AnsiC : PaperSize
    {
        public override double X => 22;
        public override double Y => 17;
        public override string PlotConfigurationName => "ANSI_full_bleed_C_(17.00_x_22.00_Inches)";
        public override string TitleBlockName { get; }
        public override string TitleBlockInformation { get; }
    }
}