using System;
using System.Windows.Input;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.Commands
{
    /// <summary>
    /// Simple command implementation for WPF buttons.
    /// Connects button clicks to ViewModel methods.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;      // What to do when button is clicked
        private readonly Predicate<object> _canExecute; // When button should be enabled

        /// <summary>
        /// Creates a command with both action and enable condition.
        /// </summary>
        /// <param name="execute">Method to run when button is clicked</param>
        /// <param name="canExecute">Method that returns true if button should be enabled</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Creates a command that is always enabled.
        /// </summary>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Checks if the command can be executed (if button should be enabled).
        /// </summary>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Executes the command (runs when button is clicked).
        /// </summary>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Event that WPF uses to know when to check if button should be enabled/disabled.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
