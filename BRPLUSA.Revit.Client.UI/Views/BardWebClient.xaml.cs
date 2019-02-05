using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Core.Interfaces;
using BRPLUSA.Revit.Services.Web;
using CefSharp;
using CefSharp.Wpf;

namespace BRPLUSA.Revit.Client.UI.Views
{
    /// <summary>
    /// Interaction logic for BardWebClient.xaml
    /// </summary>
    public partial class BardWebClient : Page, IDockablePaneProvider, ISocketConsumer, IDisposable
    {
        public static DockablePaneId Id => new DockablePaneId(new Guid());
        public static UIControlledApplication App { get; private set; }
        private ISocketProvider Socket { get; set; }

        public BardWebClient(UIControlledApplication app)
        {
            try
            {
                InitializeComponent();
                App = app;
            }
            catch(Exception e)
            {
                LoggingService.LogError("Could not initialize the WebClient", e);
            }
        }

        public void NavigateTo(string url)
        {
            Browser.Load(url);
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            data.VisibleByDefault = true;
            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
                TabBehind = DockablePanes.BuiltInDockablePanes.PropertiesPalette
            };
        }

        public void JoinRevitSession(object sender, DocumentOpenedEventArgs args)
        {
            NavigateTo(Socket.ClientUri);
            //Browser.ShowDevTools(); for DEBUG ONLY
        }

        public void ShowSidebar(object sender, DocumentOpenedEventArgs args)
        {
            var pane = App.GetDockablePane(Id);
            pane.Show();
            args.Document.Application.DocumentOpened -= ShowSidebar;
        }

        public void Register(ISocketProvider service, Document doc)
        {
            Socket = service;
        }

        public void Deregister()
        {
            App.ControlledApplication.DocumentOpened -= JoinRevitSession;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void InitializeCefSharp()
        {
            var location = Path.GetDirectoryName(typeof(BardWebClient).Assembly.Location);
            var settings = new CefSettings();

            settings.SetOffScreenRenderingBestPerformanceArgs();
            
            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(location,
                Environment.Is64BitProcess ? "x64" : "x86",
                "CefSharp.BrowserSubprocess.exe");

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }

        // Will attempt to load missing assembly from either x86 or x64 subdir
        // Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
        public static Assembly ResolveCefBinaries(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                //string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                //	Environment.Is64BitProcess ? "x64" : "x86",
                //	assemblyName);

                var location = Path.GetDirectoryName(typeof(BardWebClient).Assembly.Location);
                string archSpecificPath = Path.Combine(location,
                    Environment.Is64BitProcess ? "x64" : "x86",
                    assemblyName);

                return File.Exists(archSpecificPath)
                    ? Assembly.LoadFile(archSpecificPath)
                    : null;
            }

            return null;
        }

        public void Dispose()
        {
            Browser?.Dispose();
        }
    }
}
