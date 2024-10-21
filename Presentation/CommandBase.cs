using System.Windows.Input;

namespace WPF_MVVM_TEMPLATE.Presentation;

public class CommandBase : ICommand
{
    //Generic delegates
    protected Action<object> _executeAction;
    protected Func<object, bool> _canExecuteAction;

    /// <summary>
    /// Constructor with both execute and canExecute
    /// </summary>
    /// <param name="executeAction"></param>
    /// <param name="canExecuteAction"></param>
    public CommandBase(Action<object> executeAction, Func<object, bool> canExecuteAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = canExecuteAction;
    }

    /// <summary>
    /// Constructor with only execute
    /// </summary>
    /// <param name="executeAction"></param>
    public CommandBase(Action<object> executeAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = null;
    }

    /// <summary>
    /// Event handler, needed for the canExecute
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }



    /// <summary>
    /// Can execute, the method which enables or disables the UI element trough the Command Manager
    /// Will just forward invoke the call
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object parameter)
    {
        Boolean returnVal = true;
        if (_canExecuteAction != null)
            returnVal = _canExecuteAction(parameter);
        return returnVal;
    }

    /// <summary>
    /// The method which executes when an UI element is clicked
    /// WIll just forward invoke the call
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(object parameter)
    {
        _executeAction?.Invoke(parameter);
    }
}