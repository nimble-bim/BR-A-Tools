using System;
using System.Windows.Controls;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Client.WPF.Pages;
using BRPLUSA.Revit.Client.WPF.ViewModels;

namespace BRPLUSA.Revit.Client.WPF.Viewers
{
    /// <summary>
    /// Interaction logic for BardWpfClient.xaml
    /// </summary>
    public partial class BardWpfClient : Page, IRevitClient
    {
        private static DockablePaneId _id;
        public DockablePaneId Id => _id;
        public static UIControlledApplication App { get; private set; }
        public BackupPageContent Page { get; private set; }

        static BardWpfClient()
        {
            _id = new DockablePaneId(Guid.NewGuid());
        }

        public BardWpfClient()
        {
            try
            {
                InitializeComponent();
            }

            catch (Exception e)
            {
                LoggingService.LogError("Couldn't initialize WPF Component", e);
            }
        }

        //public BardWpfClient(BackupPageContent page)
        //{
        //    try
        //    {
        //        InitializeComponent();
        //        Page = page;
        //    }

        //    catch (Exception e)
        //    {
        //        LoggingService.LogError("Couldn't initialize WPF Component", e);
        //        throw e;
        //    }
        //}

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

    public interface IRevitClient : IDockablePaneProvider
    {
        DockablePaneId Id { get; }
        void ShowSidebar(object sender, DocumentOpenedEventArgs args);
    }
}
