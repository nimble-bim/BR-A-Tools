using System.Windows;
using System.Windows.Input;

namespace BRPLUSA.Revit.Installers._2018
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ProductInstallationService Installer { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Installer = new ProductInstallationService();
        }

        private void ShutdownPage(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnDragRequest(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void InstallRevit2018(object sender, RoutedEventArgs e)
        {
            await Installer.HandleApplicationUpgrade();
        }
    }
}
