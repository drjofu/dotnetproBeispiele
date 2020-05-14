using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfGlobalisierung.GlobalizationUtilities;

namespace WpfGlobalisierung.MVVMUtilities
{
  public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
  {
    // Die eingestellte Kultur
    private SupportedCulture currentCulture;

    public SupportedCulture CurrentCulture
    {
      get { return currentCulture; }
      set {
        if (currentCulture != value) {
          currentCulture = value;
          OnCurrentCultureChanged();
        }
      }
    }

    // Konstruktor. Richtet Kultur und Änderungsbenachrichtigung ein
    public ViewModelBase()
    {
      currentCulture = GlobalizationUtilities.GlobalizationUtilities.TheInstance.SelectedCulture;
      GlobalizationUtilities.GlobalizationUtilities.TheInstance.SelectedCultureChanged += TheInstance_SelectedCultureChanged;
    }

    // Die Kultur hat sich geändert
    void TheInstance_SelectedCultureChanged(object sender, EventArgs e)
    {
      CurrentCulture = GlobalizationUtilities.GlobalizationUtilities.TheInstance.SelectedCulture;
    }

    // Diese Methode wird bei Änderungen aufgerufen und muss von abgeleiteten ViewModels implementiert werden
    protected abstract void OnCurrentCultureChanged();

    // INotifyPropertyChanged-Implementierung
    protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    // Verknüpfung der Event-Handler wieder entfernen
    public virtual void Dispose()
    {
      GlobalizationUtilities.GlobalizationUtilities.TheInstance.SelectedCultureChanged -= TheInstance_SelectedCultureChanged;
    }
  }
}
