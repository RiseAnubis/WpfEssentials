using System.Windows.Input;

namespace WpfEssentials;

/// <summary>
/// Implementation of the interface ICommand to execute Commands
/// </summary>
public class RelayCommand : ICommand
{
    readonly Action executeHandler;
    readonly Func<bool> canExecuteHandler;

    protected RelayCommand() { }

    /// <summary>
    /// Creates a new relay command
    /// </summary>
    /// <param name="Execute">The method that should be executed by the command</param>
    /// <param name="CanExecute">The method that checks whether the command can be executed</param>
    public RelayCommand(Action Execute, Func<bool> CanExecute = null)
    {
        executeHandler = Execute ?? throw new ArgumentNullException(nameof(Execute));
        canExecuteHandler = CanExecute;
    }

    /// <summary>
    /// Checks whether the command can be executed
    /// </summary>
    /// <param name="Parameter">Optional parameter, always null in this relay command implementation</param>
    public virtual bool CanExecute(object Parameter)
    {
        if (canExecuteHandler == null)
            return true;

        return canExecuteHandler();
    }

    /// <summary>
    /// Executes the command
    /// </summary>
    /// <param name="Parameter">Optional parameter, always null in this relay command implementation</param>
    public virtual void Execute(object Parameter) => executeHandler();

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

/// <summary>
/// A relay command that takes a generic command parameter
/// </summary>
public class RelayCommand<T> : RelayCommand
{
    readonly Action<T> executeHandler;
    readonly Predicate<T> canExecuteHandler;

    /// <summary>
    /// Creates a new relay command
    /// </summary>
    /// <param name="Execute">The method that should be executed by the command</param>
    /// <param name="CanExecute">The method that checks whether the command can be executed</param>
    public RelayCommand(Action<T> Execute, Predicate<T> CanExecute = null)
    {
        executeHandler = Execute ?? throw new ArgumentException(null, nameof(Execute));
        canExecuteHandler = CanExecute;
    }

    /// <summary>
    /// Checks whether the command can be executed
    /// </summary>
    /// <param name="Parameter">The optional parameter</param>
    public override bool CanExecute(object Parameter)
    {
        if (canExecuteHandler == null)
            return true;

        return canExecuteHandler(Parameter is T parameter ? parameter : default);
    }

    /// <summary>
    /// Executes the command
    /// </summary>
    /// <param name="Parameter">The optional parameter</param>
    public override void Execute(object Parameter) => executeHandler(Parameter is T parameter ? parameter : default);
}

/// <summary>
/// A relay command for an asynchronous execution
/// </summary>
public class AsyncRelayCommand : RelayCommand
{
    readonly Func<Task> executeHandler;
    readonly Func<bool> canExecuteHandler;

    protected AsyncRelayCommand() { }

    /// <summary>
    /// Creates a new  asynchronous relay command
    /// </summary>
    /// <param name="Execute">The method that should be executed by the command</param>
    /// <param name="CanExecute">The method that checks whether the command can be executed</param>
    public AsyncRelayCommand(Func<Task> Execute, Func<bool> CanExecute = null)
    {
        executeHandler = Execute;
        canExecuteHandler = CanExecute;
    }

    /// <summary>
    /// Checks whether the command can be executed
    /// </summary>
    /// <param name="Parameter">Optional parameter, always null in this relay command implementation</param>
    public override bool CanExecute(object Parameter)
    {
        if (canExecuteHandler == null)
            return true;

        return canExecuteHandler();
    }

    /// <summary>
    /// Executes the command
    /// </summary>
    /// <param name="Parameter">Optional parameter, always null in this relay command implementation</param>
    public override async void Execute(object Parameter) => await executeHandler();
}

/// <summary>
/// An asynchronous relay command that takes a generic command parameter
/// </summary>
public class AsyncRelayCommand<T> : AsyncRelayCommand
{
    readonly Func<T, Task> executeHandler;
    readonly Func<T, bool> canExecuteHandler;

    /// <summary>
    /// Creates a new  asynchronous relay command
    /// </summary>
    /// <param name="Execute">The method that should be executed by the command</param>
    /// <param name="CanExecute">The method that checks whether the command can be executed</param>
    public AsyncRelayCommand(Func<T, Task> Execute, Func<T, bool> CanExecute = null)
    {
        executeHandler = Execute ?? throw new ArgumentException(null, nameof(Execute));
        canExecuteHandler = CanExecute;
    }

    /// <summary>
    /// Checks whether the command can be executed
    /// </summary>
    /// <param name="Parameter">The ptional parameter</param>
    public override bool CanExecute(object Parameter)
    {
        if (canExecuteHandler == null)
            return true;

        return canExecuteHandler(Parameter is T parameter ? parameter : default);
    }

    /// <summary>
    /// Executes the command
    /// </summary>
    /// <param name="Parameter">The ptional parameter</param>
    public override async void Execute(object Parameter) => await executeHandler(Parameter is T parameter ? parameter : default);
}