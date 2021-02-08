using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfMvvmMultiselect.Utilities
{
  /// <summary>
  /// Basisimplementierung INotifyPropertyChanged
  /// </summary>
  public class NotificationObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
