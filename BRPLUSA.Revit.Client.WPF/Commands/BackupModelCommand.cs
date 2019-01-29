using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BRPLUSA.Revit.Services.Updaters;

namespace BRPLUSA.Revit.Client.WPF.Commands
{
    // for more info on this pattern: https://stackoverflow.com/a/3531935/1732184
    public class BackupModelCommand
    {
        private ICommand _backupCommand;
        private readonly ManualModelBackupService _service;

        public ICommand CommandToExecute
        {
            get
            {
                return _backupCommand 
                       ?? (_backupCommand = new RelayCommand(
                           param => _service.HandleBackupRequest()
                       ));
            }
        }

        public BackupModelCommand(ManualModelBackupService service)
        {
            _service = service;
        }
    }
}
