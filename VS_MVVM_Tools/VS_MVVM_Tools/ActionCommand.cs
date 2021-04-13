using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VS_MVVM_Tools
{
  class ActionCommand : ICommand
  {
    private readonly Action action;

    public ActionCommand(Action action)
    {
      this.action = action;
    }

    private bool isEnabled;

    public bool IsEnabled
    {
      get { return isEnabled; }
      set { isEnabled = value; }
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
