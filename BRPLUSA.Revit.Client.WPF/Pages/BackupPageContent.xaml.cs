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
using BRPLUSA.Revit.Client.WPF.ViewModels;

namespace BRPLUSA.Revit.Client.WPF.Pages
{
    /// <summary>
    /// Interaction logic for BackupPageContent.xaml
    /// </summary>
    public partial class BackupPageContent : UserControl
    {
        public BackupPageContent()
        {
            InitializeComponent();
        }

        public BackupPageContent(BackupPageViewModel vm) : this()
        {
            DataContext = vm;
        }
    }
}
