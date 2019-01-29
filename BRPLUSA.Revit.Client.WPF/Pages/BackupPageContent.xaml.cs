using System.Windows.Controls;
using BRPLUSA.Revit.Client.WPF.ViewModels;

namespace BRPLUSA.Revit.Client.WPF.Pages
{
    /// <summary>
    /// Interaction logic for BackupPageContent.xaml
    /// </summary>
    public partial class BackupPageContent : UserControl
    {
        //public BackupPageContent()
        //{
        //    InitializeComponent();
        //}

        public BackupPageContent(BackupPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
