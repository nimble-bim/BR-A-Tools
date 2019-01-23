using BRPLUSA.Autodesk.Revit.WPF.Views;
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

namespace BRPLUSA.Autodesk.Revit.WPF
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            Btn_NavigateToBackup.Click += ShowNewContent;
        }

        public void ShowNewContent(object sender, RoutedEventArgs args)
        {
            SecondaryGrid.Children.Add(new BackupFragment());
        }
    }
}
