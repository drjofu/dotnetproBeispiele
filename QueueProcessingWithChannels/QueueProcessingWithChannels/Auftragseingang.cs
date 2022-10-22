using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueProcessingWithChannels
{
  public record Artikel(string Bezeichnung, int Verpackungszeit);
  public record Adresse(string Ort, int Versandzeit);
  public record Bestellung(int Auftragsnummer, IEnumerable<Artikel> Artikel, Adresse Adresse);

  public class Auftragseingang
  {
    private Artikel[] artikelliste = new Artikel[] {
      new Artikel("Hemd",2),
      new Artikel("Geschirr",5),
      new Artikel("Bild",3),
      new Artikel("Ball",1),
      new Artikel("Fernseher",4),
      new Artikel("Spiegel",4)
    };

    private Adresse[] adressen = new Adresse[]
    {
      new Adresse("Köln",1),
      new Adresse("München",1),
      new Adresse("Wien",2),
      new Adresse("Brüssel",2),
      new Adresse("London",3),
      new Adresse("Wellington",5),
      new Adresse("Tokyo",3),
      new Adresse("Ushuaia",5),
      new Adresse("Stanley",8),
      new Adresse("Edinburgh of the Seven Seas",30)
    };

    public async IAsyncEnumerable<Bestellung> GetBestellungen(int anzahl)
    {
      int auftragsnummer = 1;

      while (anzahl > 1)
      {
        anzahl--;
        auftragsnummer++;
        await Task.Delay(300);

        var n = Random.Shared.Next(1, 10);
        var artikel = new Artikel[n];
        for (int i = 0; i < n; i++) artikel[i] = artikelliste[Random.Shared.Next(artikelliste.Length)];
        var bestellung = new Bestellung(auftragsnummer, artikel, adressen[Random.Shared.Next(adressen.Length)]);
        yield return bestellung;
      }
    }
  }
}
