using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

    public void ArtikelHinzufuegen(object sender, RoutedEventArgs e)
    {
      Artikel.Add(new Artikel { Artikelnummer=42, Bezeichnung="was neues", Preis=10 });
    }

  }
}
