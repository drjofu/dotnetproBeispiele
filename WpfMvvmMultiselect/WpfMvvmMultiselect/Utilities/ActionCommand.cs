using System;
using System.Windows.Input;

namespace WpfMvvmMultiselect.Utilities
{
  /// <summary>
  /// Implementierung ICommand
  /// </summary>
  public class ActionCommand : ICommand
  {
    private readonly Action action;

    public ActionCommand(Action action)
    {
      this.action = action;
    }

    private bool isEnabled = true;

    public bool IsEnabled
    {
      get { return isEnabled; }
      set { isEnabled = value; CanExecuteChanged?.Invoke(this, EventArgs.Empty); }
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return isEnabled;
    }

    public void Execute(object parameter)
    {
      action?.Invoke();
    }
  }
}
