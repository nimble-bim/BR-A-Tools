using System.Windows;
using System.Windows.Input;

namespace BRPLUSA.Revit.Installers._2018
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProductSelectionView : Window
    {
        private InstallationManager Manager { get; set; }
        private bool Revit2018AppInstalled { get; set; }
        private bool Revit2019AppInstalled { get; set; }
        private bool Revit2018AppUpdateAvailable { get; set; }
        private bool Revit2019AppUpdateAvailable { get; set; }

        public ProductSelectionView()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            Manager = new InstallationManager();
        }

        private void InitializeProductState()
        {
            Manager.VersionHandler.
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
            await Manager.HandleRevit2018ApplicationInstallation();
        }
        private async void UpgradeRevit2018(object sender, RoutedEventArgs e)
        {
            await Manager.HandleRevit2018ApplicationUpgrade();
        }
    }
}
