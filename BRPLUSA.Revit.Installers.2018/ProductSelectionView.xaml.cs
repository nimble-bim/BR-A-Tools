using System.Windows;
using System.Windows.Input;

namespace BRPLUSA.Revit.Installers._2018
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProductSelectionView : Window
    {
        private InstallationManager Installer { get; set; }

        public ProductSelectionView()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            Installer = new InstallationManager();
        }

        public bool NeedsUpdate
        {
            get { return Installer.VersionHandler.ShouldUpdate; }
        }

        //public ProductSelectionView(InstallationManager manager) : this()
        //{
        //    Installer = manager;
        //}

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
            await Installer.HandleApplicationInstallation();
        }
        private async void UpgradeRevit2018(object sender, RoutedEventArgs e)
        {
            await Installer.HandleApplicationUpgrade();
        }
    }
}
