using System;
using System.Windows.Input;
using DragDrop.Commands.Delegates;
using DragDrop.Commands.Parameters;

namespace DragDrop.Commands
{
   public class SourceLeaveTargetCommand: ICommand
    {

        #region Constructors
        public SourceLeaveTargetCommand(SourceActionTargetCommandExecutedDelegate executed)
            : this(executed, null)
        {
        }

        public SourceLeaveTargetCommand(SourceActionTargetCommandExecutedDelegate executed, SourceActionTargetCommandCanExecuteDelegate canExecute)
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
        public bool CanExecute(object sender, SourceLeaveTargetCommandParameter parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(sender, parameter);
            }
            return true;
        }

        public void Execute(object sender, SourceLeaveTargetCommandParameter parameter)
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
            return CanExecute(null, parameter as SourceLeaveTargetCommandParameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(null, parameter as SourceLeaveTargetCommandParameter);
        }
        #endregion ICommand implementation

    }
}
