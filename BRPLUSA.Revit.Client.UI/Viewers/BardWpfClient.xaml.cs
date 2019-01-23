using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Client.WPF.Pages;
using System;
using System.Windows.Controls;

namespace BRPLUSA.Revit.Client.UI.Viewers
{
    /// <summary>
    /// Interaction logic for BardWpfClient.xaml
    /// </summary>
    public partial class BardWpfClient : Page, IDockablePaneProvider
    {
        public static DockablePaneId Id => new DockablePaneId(new Guid());
        public static UIControlledApplication App { get; private set; }

        public BardWpfClient()
        {
            InitializeComponent();
            InitializeResources();
        }

        private void InitializeResources()
        {
            Btn_Backup.Click += (sender, args) =>
            {
                ContentDisplay.Children.Add(new BackupPageContent());
            };
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

        public void ShowSidebar(object sender, DocumentOpenedEventArgs args)
        {
            var pane = App.GetDockablePane(Id);
            pane.Show();
            args.Document.Application.DocumentOpened -= ShowSidebar;
        }
    }
}
