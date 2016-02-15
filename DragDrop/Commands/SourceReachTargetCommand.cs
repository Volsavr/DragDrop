using System;
using System.Windows.Input;
using DragDrop.Commands.Delegates;
using DragDrop.Commands.Parameters;

namespace DragDrop.Commands
{
   public class SourceReachTargetCommand: ICommand
    {

        #region Constructors
        public SourceReachTargetCommand(SourceActionTargetCommandExecutedDelegate executed)
            : this(executed, null)
        {
        }

        public SourceReachTargetCommand(SourceActionTargetCommandExecutedDelegate executed,SourceActionTargetCommandCanExecuteDelegate canExecute)
        {
            ExecutedDelegate = executed;
            CanExecuteDelegate = canExecute;
        }
        
        #endregion

        #region Properties
        private SourceActionTargetCommandExecutedDelegate ExecutedDelegate
        {
            get;
            set;
        }

        private SourceActionTargetCommandCanExecuteDelegate CanExecuteDelegate
        {
            get;
            set;
        } 
        #endregion

        #region Methods
        public bool CanExecute(object sender, SourceReachTargetCommandParameter parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(sender, parameter);
            }
            return true;
        }

        public void Execute(object sender, SourceReachTargetCommandParameter parameter)
        {
            if (ExecutedDelegate != null)
            {
                ExecutedDelegate(sender, parameter);
            }
        }
        
        #endregion

        #region ICommand implementation
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(null, parameter as SourceReachTargetCommandParameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(null, parameter as SourceReachTargetCommandParameter);
        }
        #endregion ICommand implementation

    }
}
