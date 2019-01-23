//using Autodesk.Revit.DB.Events;
//using Autodesk.Revit.UI;
using System;
using System.Windows;

namespace BRPLUSA.Autodesk.Revit.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static DockablePaneId Id => new DockablePaneId(new Guid());
        //public static UIControlledApplication App { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        //public void SetupDockablePane(DockablePaneProviderData data)
        //{
        //    data.FrameworkElement = this;
        //    data.VisibleByDefault = true;
        //    data.InitialState = new DockablePaneState
        //    {
        //        DockPosition = DockPosition.Tabbed,
        //        TabBehind = DockablePanes.BuiltInDockablePanes.PropertiesPalette
        //    };
        //}

        //public void ShowSidebar(object sender, DocumentOpenedEventArgs args)
        //{
        //    var pane = App.GetDockablePane(Id);
        //    pane.Show();
        //    args.Document.Application.DocumentOpened -= ShowSidebar;
        //}
    }
}
