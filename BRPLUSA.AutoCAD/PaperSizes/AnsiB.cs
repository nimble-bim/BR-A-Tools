namespace BRPLUSA.AutoCAD.PaperSizes
{
    public class AnsiB : PaperSize
    {
        public override double X => 17;
        public override double Y => 11;
        public override string PlotConfigurationName => "ANSI_full_bleed_B_(11.00_x_17.00_Inches)";
        public override string TitleBlockName => "11X17.dwg";
        public override string TitleBlockInformation => "11X17INF.dwg";
    }
}