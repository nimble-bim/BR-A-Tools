using BRPLUSA.Revit.Client.WPF.Commands;

namespace BRPLUSA.Revit.Client.WPF.ViewModels
{
    public class BackupPageViewModel : ViewModelBase
    {
        private string _projectName;
        private string _projectDescription;

        public string ProjectName
        {
            get => _projectName;
            set => SetProperty(ref _projectName, value);
        }

        public string ProjectDescription
        {
            get => _projectDescription;
            set => SetProperty(ref _projectDescription, value);
        }
    }
}
