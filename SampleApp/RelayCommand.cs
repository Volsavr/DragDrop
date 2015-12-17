using System;
using System.Windows.Input;

namespace SampleApp
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;

        private Predicate<object> _canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this._execute = execute;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute != null && this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this._execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this._canExecute = _ => false;
            this._execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
