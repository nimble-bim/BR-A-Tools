using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BRPLUSA.Revit.Installers._2018
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProductSelectionView : Window
    {
        private const string _updateAvailable = "Update Available!";
        private const string _updateNotAvailable = "Up to date";
        private const string _productInstalled = "Installed";
        private const string _productNeedsInstall = "Install";
        private const string _productCanBeUpgraded = "Upgrade";
        private InstallationManager Manager { get; set; }

        private bool Revit2018AppInstalled
        {
            get => Manager.Revit2018AppInstalled;
            set => Revit2018AppInstallStatus.Text = value 
                ? _productInstalled
                : _productNeedsInstall;
        }

        private bool Revit2018AppUpdateAvailable
        {
            get => Manager.Revit2018AppUpdateAvailable;
            set => Revit2018UpdateStatus.Text = value
                ? _updateAvailable
                : _updateNotAvailable;
        }

        private bool Revit2019AppInstalled { get; set; }

        private bool Revit2019AppUpdateAvailable { get; set; }

        public ProductSelectionView()
        {
            InitializeComponent();
            InitializeServices();
            InitializeProductState();
        }

        private void InitializeServices()
        {
            Manager = new InstallationManager();
        }

        private void InitializeProductState()
        {
            Revit2018AppInstalled = Manager.Revit2018AppInstalled;
            Revit2018AppUpdateAvailable = Manager.Revit2018AppUpdateAvailable;
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
