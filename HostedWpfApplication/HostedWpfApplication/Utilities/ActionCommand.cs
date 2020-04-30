using System;
using System.Windows.Input;

namespace HostedWpfApplication.Utilities
{
  public class ActionCommand : ICommand
  {
    public string DisplayText { get; set; }
    public string ToolTipText { get; set; }

    private readonly Action action;
    private readonly Action<object> actionWithParam;

    private bool isEnabled = true;

    public bool IsEnabled
    {
      get { return isEnabled; }
      set { isEnabled = value; CanExecuteChanged?.Invoke(this, EventArgs.Empty); }
    }

    public ActionCommand()
    {

    }
    public ActionCommand(Action action)
    {
      this.action = action;
    }

    public ActionCommand(Action<object> actionWithParam)
    {
      this.actionWithParam = actionWithParam;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return isEnabled;
    }

    public virtual void Execute(object parameter)
    {
      action?.Invoke();
      actionWithParam?.Invoke(parameter);
    }
  }

  public class ActionCommand<T> : ActionCommand
  {
    private readonly Action<T> action;

    public ActionCommand(Action<T> action)
    {
      this.action = action;
    }

    public override void Execute(object parameter)
    {
      action?.Invoke((T)parameter);
    }
  }
}
