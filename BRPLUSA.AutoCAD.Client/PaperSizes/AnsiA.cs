namespace BRPLUSA.Client.AutoCAD.PaperSizes
{
    public class AnsiA : PaperSize
    {
        public override double X => 11;
        public override double Y => 8.5;

        public override string PlotConfigurationName => "ANSI_full_bleed_A_(8.50_x_11.00_Inches)";
        public override string TitleBlockName => "8X11.dwg";
        public override string TitleBlockInformation => "8X11INF.dwg";
    }
}