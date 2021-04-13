using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VS_MVVM_Tools
{
  class ViewModelBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
    {
      if (!Equals(field, newValue))
      {
        field = newValue;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
      }

      return false;
    }
  }
}
