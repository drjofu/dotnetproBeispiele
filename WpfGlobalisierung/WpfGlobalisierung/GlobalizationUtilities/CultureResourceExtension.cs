using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfGlobalisierung.GlobalizationUtilities
{
  public class CultureResourceExtension : MarkupExtension
  {
    public string Id { get; set; }
    public CultureResourceExtension()
    {}
    public CultureResourceExtension(string id)
    {
      this.Id = id;
    }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      Binding binding = new Binding("SelectedCulture[" + Id + "]");
      binding.Source = GlobalizationUtilities.TheInstance;
      return binding.ProvideValue(serviceProvider);
    }
  }
}
