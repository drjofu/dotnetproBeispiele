using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WpfAsyncBeispiele
{
  public class ActionCommand : ICommand
  {
    public string DisplayText { get; set; }
    public string ToolTipText { get; set; }
    public object Tag { get; set; }

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
      actionWithParam?.Invoke(parameter ?? Tag);
    }
  }

  public class ActionCommand<T> : ActionCommand 
  {

    private readonly Action<T> action;

    public ActionCommand(Action<T>action)
    {
      this.action = action;
    }

    public override void Execute(object parameter)
    {
      action?.Invoke((T)parameter ?? (T)Tag); // Command-Parameter oder, falls null, dann Tag übergeben
    }
  }
}
