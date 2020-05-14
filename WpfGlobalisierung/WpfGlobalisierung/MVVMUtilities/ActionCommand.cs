using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfGlobalisierung.MVVMUtilities
{
  public class ActionCommand:ICommand
  {
    private Action action;
    public ActionCommand(Action action)
    {
      this.action = action;
    }

    private bool isEnabled = true;

    public bool IsEnabled
    {
      get { return isEnabled; }
      set { isEnabled = value; if (CanExecuteChanged != null)CanExecuteChanged(this, EventArgs.Empty); }
    }
    public bool CanExecute(object parameter)
    {
      return isEnabled;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
      if (action != null) action();
    }
  }
}
