using System.Collections.ObjectModel;
using WpfMvvmMultiselect.Models;
using WpfMvvmMultiselect.Utilities;

namespace WpfMvvmMultiselect
{
  public class ViewModel : NotificationObject
  {
    /// <summary>
    /// Liste auswählbarer Objekte
    /// </summary>
    public ObservableCollection<SelectableItem> Liste { get; set; }

    /// <summary>
    /// Kommando Volltextsuche
    /// </summary>
    public ActionCommand VolltextsucheCommand { get; set; }

    /// <summary>
    /// Eingabe für die Volltextsuche
    /// </summary>
    public string Suchfeld { get; set; }

    /// <summary>
    /// ctor
    /// </summary>
    public ViewModel()
    {
      // Liste aus Demodaten erstellen
      Liste = SelectableItemList.FromItems(new Personenliste());

      // Command anlegen
      VolltextsucheCommand = new ActionCommand(Volltextsuche);
      Suchfeld = "Schm";
    }

    private void Volltextsuche()
    {
      // IsSelected gemäß Suchergebnis setzen
      foreach (var item in Liste)
      {
        var person = (Person)item.Data;
        string text = $"{person.Vorname}_{person.Nachname}_{person.Alter}";
        item.IsSelected = text.Contains(Suchfeld);
      }
    }

  }
}
