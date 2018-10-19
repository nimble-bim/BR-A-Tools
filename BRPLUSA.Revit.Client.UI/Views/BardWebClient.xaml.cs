using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Services.Web;
using CefSharp;
using CefSharp.Wpf;

namespace BRPLUSA.Revit.Client.UI.Views
{
    /// <summary>
    /// Interaction logic for BardWebClient.xaml
    /// </summary>
    public partial class BardWebClient : IDockablePaneProvider
    {
        //private string _id = "{F791F35B-91E2-493B-8C9B-DDC0437C1C9D}";
        //private static string _id = new Guid().ToString();
        public static DockablePaneId Id => new DockablePaneId(new Guid());
        private static UIControlledApplication App { get; set; }
        private SocketService Socket { get; set; }
        //BrowserView 

        public BardWebClient()
        {
            InitializeComponent();
            InitializeWebComponents();
        }

        public BardWebClient(SocketService service) : this()
        {
            Socket = service;
        }

        private void InitializeWebComponents()
        {
            //View.Navigate(new Uri("http://www.google.com"));
        }

        public void NavigateTo(string url)
        {
            //WebBrowser1.Browser.LoadURL(url);
        }

        public void JoinRevitSession(object sender, DocumentOpenedEventArgs args)
        {
            NavigateTo($@"http://localhost:4001/?room={Socket.Id}");
        }

        public void Browse(object sender, DocumentOpenedEventArgs args)
        {
            //View.Navigate(new Uri("http://www.google.com"));
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            data.InitialState = new DockablePaneState
            {
                //DockPosition = DockPosition.Right,
                DockPosition = DockPosition.Tabbed,
                TabBehind = DockablePanes.BuiltInDockablePanes.PropertiesPalette
            };
        }

        public void ShowSidebar(object sender, DocumentOpenedEventArgs e)
        {
            var pane = App.GetDockablePane(Id);
            pane.Show();
            App.ControlledApplication.DocumentOpened -= ShowSidebar;
        }

        public void RegisterEvents(UIControlledApplication app)
        {
            App = app;
            App.ControlledApplication.DocumentOpened += ShowSidebar;
            App.ControlledApplication.DocumentOpened += Browse;
            //App.ControlledApplication.DocumentOpened += JoinRevitSession;
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
        public static Assembly Resolver(object sender, ResolveEventArgs args)
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
    }
}
