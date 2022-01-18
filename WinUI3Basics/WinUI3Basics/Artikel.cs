using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinUI3Basics
{
  public class Artikel : INotifyPropertyChanged
  {
    public int Artikelnummer { get; set; }
    public string Bezeichnung { get; set; }
    private double preis;

    public double Preis
    {
      get { return preis; }
      set
      {
        if (preis == value) return;
        preis = value;
        OnPropertyChanged();
      }
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new(propertyName));
    }
  }

  public class Laden
  {
    public string Name { get; set; } = "Hinz & Kunz";
    public ObservableCollection<Artikel> Artikel { get; set; } = new()
    {
      new Artikel { Artikelnummer = 1, Bezeichnung = "Ball", Preis = 12 },
      new Artikel { Artikelnummer = 4, Bezeichnung = "Telefon", Preis = 49 },
      new Artikel { Artikelnummer = 6, Bezeichnung = "Lautsprecher", Preis = 23 },
      new Artikel { Artikelnummer = 71, Bezeichnung = "Tasse", Preis = 4 },
      new Artikel { Artikelnummer = 18, Bezeichnung = "Teller", Preis = 7 }
    };

    // Event-Handler, über {x:Bind} gebunden:
    // mit Parametern:
    // public void ArtikelHinzufuegen(object sender, RoutedEventArgs e)
    // oder ohne:
    public void ArtikelHinzufuegen()
    {
      Artikel.Add(new Artikel { Artikelnummer=42, Bezeichnung="was neues", Preis=10 });
    }

  }
}
