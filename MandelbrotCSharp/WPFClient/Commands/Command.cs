using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WPFClient.Commands
{
    public class Command : ICommand
    {
        /// <summary>
        /// The action that should be execute.
        /// </summary>
        private readonly Action<object> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command" /> class.
        /// </summary>
        /// <param name="action">The command input.</param>
        public Command(Action<object> action)
        {
            this.action = action;
        }

        /// <summary>
        /// The event if the execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// This method checks if the command is execute.
        /// </summary>
        /// <param name="parameter">The input parameter.</param>
        /// <returns>Returns if the command can be execute.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// This method execute.
        /// </summary>
        /// <param name="parameter">The input parameter.</param>
        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}