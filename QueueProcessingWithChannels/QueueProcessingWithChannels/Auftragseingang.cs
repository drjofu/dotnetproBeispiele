using System.Runtime.CompilerServices;

namespace QueueProcessingWithChannels
{
  public record Artikel(string Bezeichnung, int Verpackungszeit);
  public record Adresse(string Ort, int Versandzeit);
  public record Bestellung(int Auftragsnummer, IEnumerable<Artikel> Artikel, Adresse Adresse);

  public class Auftragseingang
  {
    private readonly Artikel[] artikelliste = new Artikel[]
    {
      new Artikel("Hemd",2),
      new Artikel("Geschirr",5),
      new Artikel("Bild",3),
      new Artikel("Ball",1),
      new Artikel("Fernseher",4),
      new Artikel("Spiegel",4)
    };

    private readonly Adresse[] adressen = new Adresse[]
    {
      new Adresse("Köln",1),
      new Adresse("München",1),
      new Adresse("Hamburg",1),
      new Adresse("Bremen",1),
      new Adresse("Dresden",1),
      new Adresse("Wien",2),
      new Adresse("Brüssel",2),
      new Adresse("London",3),
      new Adresse("Wellington",5),
      new Adresse("Tokyo",3),
      new Adresse("Ushuaia",5),
      new Adresse("Stanley",8),
      new Adresse("Edinburgh of the Seven Seas",30)
    };

    /// <summary>
    /// Bestellungen generieren und als AsyncEnumerable bereitstellen
    /// </summary>
    /// <param name="anzahl">Anzahl der gewünschten Bestellungen</param>
    /// <param name="cancellationToken">Abbruch-Token</param>
    /// <returns>Bestellungen</returns>
    public async IAsyncEnumerable<Bestellung> GetBestellungen(
      int anzahl,
      [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
      int auftragsnummer = 0;

      // Gewünschte Anzahl Bestellungen generieren
      while (anzahl >= 1)
      {
        anzahl--;
        auftragsnummer++;

        // try/catch nur, damit der Debugger hier nicht stoppt
        try
        {
          // Abstand zwischen Bestellungseingängen simulieren
          await Task.Delay(300, cancellationToken);
        }
        catch (Exception)
        {
          Console.WriteLine("Bestellungsannahme abgebrochen");
          yield break;
        }

        // Artikel zusammenstellen
        var n = Random.Shared.Next(1, 10);
        var artikel = new Artikel[n];
        for (int i = 0; i < n; i++) artikel[i] = artikelliste[Random.Shared.Next(artikelliste.Length)];

        // Bestellung vervollständigen
        var bestellung = new Bestellung(auftragsnummer, artikel, adressen[Random.Shared.Next(adressen.Length)]);

        // aktuelle Iteration abschließen
        yield return bestellung;
      }

    }
  }
}
