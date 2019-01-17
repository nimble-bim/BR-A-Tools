using BRPLUSA.Revit.Services.Updates;
using System.Windows.Controls;

namespace BRPLUSA.Revit.Client.UI.Pages
{
    /// <summary>
    /// Interaction logic for ModelBackupPage.xaml
    /// </summary>
    public partial class ModelBackupPage : Page
    {
        public ModelBackupPage()
        {
            InitializeComponent();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            Btn_Command_Backup.Click += (obj, e) => ModelBackupService.HandleBackupRequest();
        }
    }
}
