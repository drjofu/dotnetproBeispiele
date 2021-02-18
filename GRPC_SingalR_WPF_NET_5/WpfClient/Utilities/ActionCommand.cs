using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClient.Utilities
{
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
      action();
    }
  }
}
