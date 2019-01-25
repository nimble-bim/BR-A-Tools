using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public void Thing()
        {
            var cmd = new BackupModelCommand(OtherThing);
        }

        public void OtherThing()
        {

        }
    }
}
