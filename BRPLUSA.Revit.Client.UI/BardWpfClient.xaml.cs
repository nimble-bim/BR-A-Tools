using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Services.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BRPLUSA.Revit.Client.UI
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
            InitializeResources()
        }

        private void InitializeResources()
        {
            
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
