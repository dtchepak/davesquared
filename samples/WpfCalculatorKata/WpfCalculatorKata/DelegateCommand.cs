using System;
using System.Windows.Input;

namespace WpfCalculatorKata
{
    public class DelegateCommand : ICommand {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        protected void OnCanExecuteChanged(EventArgs e)
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, e);
        }
    }
}