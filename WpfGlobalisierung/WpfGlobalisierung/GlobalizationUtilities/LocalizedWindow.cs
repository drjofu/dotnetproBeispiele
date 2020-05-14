using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfGlobalisierung.GlobalizationUtilities
{
  public class LocalizedWindow : Window
  {
    public LocalizedWindow()
    {
      Binding binding = new Binding("SelectedCulture.XmlLanguage");
      binding.Source = GlobalizationUtilities.TheInstance;
      this.SetBinding(LanguageProperty, binding);

      binding = new Binding("SelectedCulture.FlowDirection");
      binding.Source = GlobalizationUtilities.TheInstance;
      this.SetBinding(FlowDirectionProperty, binding);

    }
  }
}
