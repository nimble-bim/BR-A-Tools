using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BRPLUSA.Revit.Installers._2018.Services;
using RSG;

namespace BRPLUSA.Revit.Installers._2018
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProductSelectionView : Window, IDisposable
    {
        private const string _updateAvailable = "Update Available!";
        private const string _updateNotAvailable = "Up to date";
        private const string _productInstalled = "Installed";
        private const string _productNeedsInstall = "Install";
        private const string _productCanBeUpgraded = "Upgrade";

        private InstallManager Manager { get; set; }

        private bool Revit2018Installed { get; set; }
        private bool AppFor2018Installed { get; set; }
        private bool AppFor2018Installing { get; set; }

        private bool AppFor2018HasUpdateAvailable
        {
            get => Manager.Revit2018AppUpdateAvailable;
        }

        public ProductSelectionView()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            Manager = new InstallManager();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await InitializeProductState();
        }

        private async Task InitializeProductState()
        {
            await Manager.InitializeProductState();
            SetInstallationStatuses();
        }

        private void SetInstallationStatuses()
        {
            SetRevit2018Status(Manager.Revit2018AppInstalled);
            SetV2018InstallationStatus(Manager.Revit2018AppInstalled);
            SetV2018UpdateAvailability(Manager.Revit2018AppUpdateAvailable);

            if (!Revit2018Installed)
                ReportRevit2018NotInstalled();
        }

        private void ReportRevit2018NotInstalled()
        {
            ShowRevit2018NotInstalled();
            ShowRevit2018InstallationFailed();
            Revit2018UpdateStatus.Text = "Revit 2018 is not installed!";
        }

        private void SetRevit2018Status(bool status)
        {
            AppFor2018Installed = status;
        }

        private void ShutdownPage(object sender, RoutedEventArgs e)
        {
            Close();
            Dispose();
        }

        private void OnDragRequest(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void InstallRevit2018(object sender, RoutedEventArgs e)
        {
            if (AppFor2018Installed || AppFor2018Installing)
                return;

            ShowRevit2018InstallationInProcess();

            //var success = await Manager.HandleRevit2018ApplicationInstallation();

            var promise = new Promise(
                async (resolve, reject) =>
                {
                    var success = await Manager.HandleRevit2018ApplicationInstallation();

                    if (success)
                    {
                        resolve();
                    }
                    else
                    {
                        reject(null);
                    }
                }
            );

            promise.Done(ShowRevit2018InstallationComplete,
                (err) => ShowRevit2018InstallationFailed());
        }

        private void ShowRevit2018InstallationComplete()
        {
            AppFor2018Installing = false;
            ButtonRevit2018AppInstallStatus.Background = new SolidColorBrush(Color.FromRgb(51,157,255));
            ButtonRevit2018AppInstallStatus.Content = "Installed";
        }

        private void ShowRevit2018InstallationInProcess()
        {
            AppFor2018Installing = true;
            ButtonRevit2018AppInstallStatus.Background = Brushes.Gray;
            ButtonRevit2018AppInstallStatus.Content = "Installing...";
        }

        private void ShowRevit2018InstallationFailed()
        {
            AppFor2018Installing = false;
            ButtonRevit2018AppInstallStatus.Background = Brushes.Crimson;
            ButtonRevit2018AppInstallStatus.Foreground = Brushes.White;
            ButtonRevit2018AppInstallStatus.Content = "Failed";
        }

        private void ShowRevit2018NotInstalled()
        {
            throw new NotImplementedException();
        }

        private void SetV2018InstallationStatus(bool status)
        {
            AppFor2018Installed = status;
            ButtonRevit2018AppInstallStatus.Content = status
                ? _productInstalled
                : _productNeedsInstall;
        }

        private void SetV2018UpdateAvailability(bool status)
        {
            Revit2018UpdateStatus.Text = status
                ? _updateAvailable
                : _updateNotAvailable;

            if (Revit2018Installed)
                ButtonRevit2018AppInstallStatus.Content = _productCanBeUpgraded;
        }

        public void Dispose()
        {
            Manager.Dispose();
        }
    }
}
