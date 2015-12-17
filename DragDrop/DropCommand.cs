using System;
using System.Windows.Input;

namespace DragDrop
{
    /// <summary>
    /// Represents a drop command
    /// </summary>
    public class DropCommand : ICommand
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the DropCommand class by the given
        /// parameter
        /// </summary>
        /// <param name="executed">
        /// Executed delegate
        /// </param>
        public DropCommand(DropCommandExecutedDelegate executed)
            : this(executed, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DropCommand class by the given
        /// parameter
        /// </summary>
        /// <param name="executedd">
        /// Executed delegate
        /// </param>
        /// <param name="canExecute">
        /// Can execute delegate
        /// </param>
        public DropCommand(DropCommandExecutedDelegate executed, DropCommandCanExecuteDelegate canExecute)
        {
            ExecutedDelegate = executed;
            CanExecuteDelegate = canExecute;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the executed delegate
        /// </summary>
        private DropCommandExecutedDelegate ExecutedDelegate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the can execute delegate
        /// </summary>
        private DropCommandCanExecuteDelegate CanExecuteDelegate
        {
            get;
            set;
        } 
        #endregion

        #region Methods

        /// <summary>
        /// Checks if the command can be executed
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="parameter">
        /// Command parameter
        /// </param>
        /// <returns>
        /// True if it can be executed, fals otherwise
        /// </returns>
        public bool CanExecute(object sender, DropCommandParameter parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(sender, parameter);
            }
            return true;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="parameter">
        /// Command parameter
        /// </param>
        public void Execute(object sender, DropCommandParameter parameter)
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
            return CanExecute(null, parameter as DropCommandParameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(null, parameter as DropCommandParameter);
        }
        #endregion ICommand implementation

    }

}
