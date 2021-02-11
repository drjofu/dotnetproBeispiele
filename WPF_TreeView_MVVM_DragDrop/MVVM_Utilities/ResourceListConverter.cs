using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MVVM_Utilities
{
  /// <summary>
  /// Brücke zwischen einer Auflistung im XAML-Code und einem über eine Datenbindung übergebenen Index
  /// </summary>
  public class ResourceListConverter : IValueConverter
  {
    /// <summary>
    /// Liste der verfügbaren Objekte
    /// </summary>
    public List<object> Items { get; set; } = new List<object>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      // Erwartet wird ein Index vom Typ Int32
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
