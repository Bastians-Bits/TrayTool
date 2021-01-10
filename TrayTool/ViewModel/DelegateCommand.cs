using System;
using System.Windows.Input;

namespace TrayTool.ViewModel
{ 
    /// <summary>
    /// Simple interface delegation to use in the MVVC pattern
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _executeAction;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
        }

        public void Execute(object parameter) => _executeAction(parameter);

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
