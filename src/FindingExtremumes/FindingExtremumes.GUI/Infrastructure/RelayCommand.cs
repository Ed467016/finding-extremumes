using System;
using System.Windows.Input;

namespace FindingExtremumes.GUI.Infrastructure
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Func<object, bool> _canExecute;
        private Action<object> _execute;

        public RelayCommand(
            Func<object, bool> canExecute,
            Action<object> execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public RelayCommand(Action<object> execute)
        {
            this._canExecute = o => true;
            this._execute = execute;
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
