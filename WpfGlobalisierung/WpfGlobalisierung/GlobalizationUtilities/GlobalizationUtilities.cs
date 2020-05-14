using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfGlobalisierung.GlobalizationUtilities
{
  public class GlobalizationUtilities : INotifyPropertyChanged
  {
    // Singleton-Referenz
    public static readonly GlobalizationUtilities TheInstance 
      = new GlobalizationUtilities();

    static GlobalizationUtilities() { }

    // Liste der unterstützten Kulturen
    public List<SupportedCulture> SupportedCultures { get; set; }

    // Ausgewählte Kultur
    public SupportedCulture SelectedCulture { get; set; }
    public event EventHandler SelectedCultureChanged;

    // Konstruktor. Einrichten der verfügbaren Kulturen
    public GlobalizationUtilities()
    {
      SupportedCultures = new List<SupportedCulture>();
      SupportedCultures.Add(new SupportedCulture("de-DE", GlobalizationUtilities_IsSelectedChanged));
      SupportedCultures.Add(new SupportedCulture("en-GB", GlobalizationUtilities_IsSelectedChanged));
      SupportedCultures.Add(new SupportedCulture("en-US", GlobalizationUtilities_IsSelectedChanged));
      SupportedCultures.Add(new SupportedCulture("fr-FR", GlobalizationUtilities_IsSelectedChanged));
      SupportedCultures.Add(new SupportedCulture("fa-IR", GlobalizationUtilities_IsSelectedChanged));

      this.SelectedCulture = SupportedCultures.FirstOrDefault();
      
    }

    void GlobalizationUtilities_IsSelectedChanged(object sender, EventArgs e)
    {
      SupportedCulture culture = sender as SupportedCulture;
      if (culture.IsSelected)
        SelectCulture(culture);
    }

    private void SelectCulture(SupportedCulture culture)
    {
      foreach (var c in SupportedCultures)
      {
        if (c != culture) c.IsSelected = false;
      }
      SelectedCulture = culture;

      Thread.CurrentThread.CurrentCulture = culture.CultureInfo;
      Thread.CurrentThread.CurrentUICulture = culture.CultureInfo;

      if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(null));
      if (SelectedCultureChanged != null) SelectedCultureChanged(this, EventArgs.Empty);
    }

    

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
