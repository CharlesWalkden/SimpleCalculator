using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleCalculator
{
    public class RelayCommand : ICommand
    {
        private Action? mAction;
        private object? navigate;

        public event EventHandler? CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action? action)
        {
            mAction = action;
        }

        public RelayCommand(object? navigate)
        {
            this.navigate = navigate;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            mAction?.Invoke();
        }
    }
    public class RelayCommand<T> : ICommand
    {
        private Action<T>? mAction;
        private object? navigate;

        public event EventHandler? CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action<T>? action)
        {
            mAction = action;
        }

        public RelayCommand(object? navigate)
        {
            this.navigate = navigate;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            mAction?.Invoke((T?)parameter);
        }
    }
}
