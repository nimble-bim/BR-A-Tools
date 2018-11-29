using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
        private InstallationManager Manager { get; set; }

        private bool Revit2018AppInstalled
        {
            get => Manager.Revit2018AppInstalled;
            set
            {
                ButtonRevit2018AppInstallStatus.Content = value
                    ? _productInstalled
                    : _productNeedsInstall;
            }
        }

        private bool Revit2018AppUpdateAvailable
        {
            get => Manager.Revit2018AppUpdateAvailable;
            set
            {
                if (Revit2018AppInstalled)
                {
                    Revit2018UpdateStatus.Text = value
                        ? _updateAvailable
                        : _updateNotAvailable;

                    ButtonRevit2018AppInstallStatus.Content = _productCanBeUpgraded;
                }

                Revit2018UpdateStatus.Text = string.Empty;
            }
        }

        private bool Revit2019AppInstalled { get; set; }

        private bool Revit2019AppUpdateAvailable { get; set; }

        public ProductSelectionView()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            Manager = new InstallationManager();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await InitializeProductState();
        }

        private async Task InitializeProductState()
        {
            await Manager.InitializeProductState();
            Revit2018AppInstalled = Manager.Revit2018AppInstalled;
            Revit2018AppUpdateAvailable = Manager.Revit2018AppUpdateAvailable;
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
            ShowRevit2018InstallationInProcess();

            //await Manager.HandleRevit2018ApplicationInstallation();

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
            ButtonRevit2018AppInstallStatus.Background = new SolidColorBrush(Color.FromRgb(51,157,255));
            ButtonRevit2018AppInstallStatus.Content = "Installed";
        }

        private void ShowRevit2018InstallationInProcess()
        {
            ButtonRevit2018AppInstallStatus.Background = Brushes.Gray;
            ButtonRevit2018AppInstallStatus.Content = "Installing...";
        }

        private void ShowRevit2018InstallationFailed()
        {
            ButtonRevit2018AppInstallStatus.Background = Brushes.Crimson;
            ButtonRevit2018AppInstallStatus.Foreground = Brushes.White;
            ButtonRevit2018AppInstallStatus.Content = "Failed";
        }

        private async void UpgradeRevit2018(object sender, RoutedEventArgs e)
        {
            await Manager.HandleRevit2018ApplicationUpgrade();
        }

        public void Dispose()
        {
            Manager.Dispose();
        }
    }
}
