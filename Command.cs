using System;
using System.Windows.Input;

namespace Game2048
{
    public class Command : ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExecute;

        public Command(Action<object> execute)
        {
            this.execute = execute;
            canExecute = o => true;
        }
        public Command(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
