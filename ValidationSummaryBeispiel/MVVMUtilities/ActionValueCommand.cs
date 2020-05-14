using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMUtilities
{
  /// <summary>
  /// ICommand-Implementierung mit zusätzlicher Value-Eigenschaft
  /// </summary>
  /// <typeparam name="TValue">Typ der Value-Eigenschaft</typeparam>
  public class ActionValueCommand<TValue> : ICommand
  {
    private Action<TValue> action;
    private TValue value;

    public TValue Value
    {
      get { return this.value; }
      set { this.value = value; }
    }

    public ActionValueCommand(Action<TValue> action, TValue value)
    {
      this.action = action;
      this.value = value;
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
      // parameter wird ignoriert und stattdessen der Wert von Value zurückgegeben
      action(value);
    }
  }
}
