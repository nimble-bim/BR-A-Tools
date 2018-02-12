namespace BRPLUSA.AutoCAD.PaperSizes
{
    public abstract class PaperSize
    {
        protected string _newPlotter;
        protected const string _plotterDefault = "DWG To PDF.pc3";
        public abstract double X { get; }
        public abstract double Y { get; }

        public string Plotter
        {
            get
            {
                return string.IsNullOrEmpty(_newPlotter) 
                    ? _plotterDefault 
                    : _newPlotter;
            }
        }

        public abstract string PlotConfigurationName { get; }
        public abstract string TitleBlockName { get; }
        public abstract string TitleBlockInformation { get; }

        public double[] SizeValue => new[] { X, Y };

        protected void SetNewPlotter(string newPlot)
        {
            _newPlotter = newPlot;
        }
    }
}
