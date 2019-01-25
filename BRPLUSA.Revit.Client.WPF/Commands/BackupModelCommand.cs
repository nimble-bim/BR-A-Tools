using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BRPLUSA.Revit.Client.WPF.Commands
{
    // for more info on this pattern: https://stackoverflow.com/a/3531935/1732184
    public class BackupModelCommand
    {
        private ICommand _backupCommand;
        private readonly Action _execute;

        public ICommand CommandToExecute
        {
            get
            {
                return _backupCommand 
                       ?? (_backupCommand = new RelayCommand(
                           param => _execute()
                       ));
            }
        }

        public BackupModelCommand(Action toTake)
        {
            _execute = toTake;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        [DebuggerStepThrough]
        public bool CanExecute(object parameters)
        {
            return _canExecute?.Invoke(parameters) ?? true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameters)
        {
            _execute(parameters);
        }
    }
}
