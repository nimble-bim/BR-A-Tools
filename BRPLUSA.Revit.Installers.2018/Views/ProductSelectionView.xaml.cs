using System.Windows;
using System.Windows.Input;

namespace BRPLUSA.Revit.Installers._2018.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ProductInstallationApplication Installer { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Installer = new ProductInstallationApplication();
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
            Installer.ConfigureAppEvents();
            await Installer.HandleApplicationUpgrade();
        }
    }
}
