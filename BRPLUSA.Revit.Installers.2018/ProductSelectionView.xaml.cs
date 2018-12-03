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
        private bool AppFor2018CanInstall { get; set; }
        private bool AppFor2018HasUpdateAvailable { get; set; }

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
            await SetInstallationStatuses();
        }

        private async Task SetInstallationStatuses()
        {
            SetRevit2018InstallStatus(Manager.Revit2018Installed);

            if (!Revit2018Installed)
            {
                ShowRevit2018NotInstalled();
                return;
            }

            await Manager.InitializeProductState();
            SetAppFor2018InstallStatus(Manager.AppFor2018Installed);
            SetAppFor2018UpdateAvailability(Manager.AppFor2018HasUpdateAvailable);
        }

        private void SetRevit2018InstallStatus(bool status)
        {
            Revit2018Installed = status;
            AppFor2018CanInstall = status;
        }

        private void ShowAppFor2018InstallationComplete()
        {
            AppFor2018Installing = false;
            AppFor2018CanInstall = false;
            ButtonRevit2018AppInstallStatus.Background = new SolidColorBrush(Color.FromRgb(51,157,255));
            ButtonRevit2018AppInstallStatus.Content = "Installed";
        }

        private void ShowAppFor2018InstallationInProcess()
        {
            AppFor2018Installing = true;
            AppFor2018CanInstall = false;
            ButtonRevit2018AppInstallStatus.Background = Brushes.Gray;
            ButtonRevit2018AppInstallStatus.Content = "Installing...";
        }

        private void ShowAppFor2018InstallationFailed()
        {
            AppFor2018Installing = false;
            AppFor2018CanInstall = true;
            ButtonRevit2018AppInstallStatus.Background = Brushes.Crimson;
            ButtonRevit2018AppInstallStatus.Foreground = Brushes.White;
            ButtonRevit2018AppInstallStatus.Content = "Failed";
        }

        private void ShowRevit2018NotInstalled()
        {
            ButtonRevit2018AppInstallStatus.Background = Brushes.Gray;
            ButtonRevit2018AppInstallStatus.Foreground = Brushes.White;
            ButtonRevit2018AppInstallStatus.Content = "Can't Install";

            Revit2018UpdateStatus.Foreground = Brushes.Crimson;
            Revit2018UpdateStatus.Text = "Revit 2018 Not Installed";

            AppFor2018CanInstall = false;
        }

        private void SetAppFor2018InstallStatus(bool status)
        {
            AppFor2018Installed = status;
            AppFor2018CanInstall = !status;
            ButtonRevit2018AppInstallStatus.Content = status
                ? _productInstalled
                : _productNeedsInstall;
        }

        private void SetAppFor2018UpdateAvailability(bool status)
        {
            AppFor2018HasUpdateAvailable = status;
            Revit2018UpdateStatus.Text = status
                ? _updateAvailable
                : _updateNotAvailable;

            if (AppFor2018Installed)
                ButtonRevit2018AppInstallStatus.Content = status
                    ? _productCanBeUpgraded
                    : _productInstalled;
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
            if (!AppFor2018CanInstall)
                return;

            ShowAppFor2018InstallationInProcess();

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

            promise.Done(
                ShowAppFor2018InstallationComplete,
                (err) => ShowAppFor2018InstallationFailed()
                );
        }

        public void Dispose()
        {
            Manager.Dispose();
        }
    }
}
