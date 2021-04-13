using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VS_MVVM_Tools
{
  class AAANochNCommand : ICommand
  {
    private readonly Action action;

    public AAANochNCommand(Action action)
    {
      this.action = action;
    }
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      throw new NotImplementedException();
    }

    public void Execute(object parameter)
    {
      throw new NotImplementedException();
    }
  }
}
