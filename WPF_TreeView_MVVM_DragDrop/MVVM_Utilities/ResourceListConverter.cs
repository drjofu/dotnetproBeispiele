using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MVVM_Utilities
{
  public class ResourceListConverter : IValueConverter
  {
    public List<object> Items { get; set; } = new List<object>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int index = (int)value;
      if (index >= Items.Count) index = 0;
      return Items[index];  // Bei Bedarf Fehler abfangen
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
