using System;
using System.Windows.Input;

namespace FindingExtremumes.GUI.Infrastructure
{
    /// <summary>
    /// Stands for command pattern. MVVM action binding mechanism.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action<object> action;
        private Func<object, bool> func;

        public RelayCommand(
            Action<object> action,
            Func<object, bool> func)
        {
            this.action = action;
            this.func = func;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return func(parameter);
        }

        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
