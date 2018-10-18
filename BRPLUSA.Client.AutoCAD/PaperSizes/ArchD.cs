namespace BRPLUSA.AutoCAD.PaperSizes
{
    public class ArchD : PaperSize
    {
        public override double X => 36;
        public override double Y => 24;
        public override string PlotConfigurationName => "ARCH_full_bleed_D_(24.00_x_36.00_Inches)";
        public override string TitleBlockName => "24X36.dwg";
        public override string TitleBlockInformation => "24X36INF.dwg";
    }
}