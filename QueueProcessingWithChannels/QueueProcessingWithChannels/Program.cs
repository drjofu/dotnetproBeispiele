using QueueProcessingWithChannels;
using System.Diagnostics;
using System.Threading.Channels;

public class Program
{

  static Stopwatch stopwatch = Stopwatch.StartNew();
  static Channel<Bestellung> channelAuftragseingang = Channel.CreateUnbounded<Bestellung>();
  static Channel<Bestellung> channelVersandInland = Channel.CreateBounded<Bestellung>(new BoundedChannelOptions(5) { SingleReader = true });
  static Channel<Bestellung> channelVersandAusland = Channel.CreateBounded<Bestellung>(2);

  static Auftragseingang auftragseingang = new Auftragseingang();
  static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
  static CancellationToken cancellationToken = cancellationTokenSource.Token;

  public static async Task Main()
  {
    AbbruchDurchBenutzerÜberwachen();

    var tasks = new List<Task>();

    tasks.Add(BestellungenVerarbeiten());

    for (int i = 0; i < 3; i++)
    {
      tasks.Add(VerpackungsteamStarten());
    }

    tasks.Add(PaketversandAusführen(channelVersandInland));
    tasks.Add(PaketversandAusführen(channelVersandAusland));

    await Task.WhenAll(tasks);

    if (cancellationToken.IsCancellationRequested)
      Print("Vorzeitiger Abbruch", ConsoleColor.Red);
    else
      Print("Alles erledigt", ConsoleColor.Green);


  }

  private static async Task PaketversandAusführen(Channel<Bestellung> channelVersand)
  {
    try
    {
      Print($"Logistiker beginnt mit dem Versenden der Pakete", ConsoleColor.Cyan);
      await foreach (var item in channelVersand.Reader.ReadAllAsync(cancellationToken))
      {
        Print($"Versand für {item.Auftragsnummer} begonnen", ConsoleColor.Cyan);
        await Task.Delay(item.Adresse.Versandzeit * 1000);
        Print($"Bestellung {item.Auftragsnummer} in {item.Adresse.Ort} ausgeliefert", ConsoleColor.Cyan);
      }
      Print($"Logistiker hat alle Versandaufträge abgeschlossen", ConsoleColor.Cyan);
    }
    catch (OperationCanceledException)
    {
      Print("Logistiker hat Arbeit abgebrochen", ConsoleColor.Red);
    }
  }

  private static void AbbruchDurchBenutzerÜberwachen()
  {
    Task.Run(() =>
    {

      Console.WriteLine("Zum Beenden Eingabetaste drücken");
      Console.ReadLine();
      Console.WriteLine("Abbruch aller Vorgänge");
      cancellationTokenSource.Cancel();
    });
  }

  static async Task VerpackungsteamStarten()
  {
    try
    {
      await foreach (var bestellung in channelAuftragseingang.Reader.ReadAllAsync(cancellationToken))
      {
        await Verpacken(bestellung);
      }

      Print("Verpackungsteam ist fertig", ConsoleColor.Green);

    }
    catch (OperationCanceledException)
    {
      Print("Verpackungsteam hat Arbeit abgebrochen", ConsoleColor.Red);
    }
  }

  static async Task BestellungenVerarbeiten()
  {
    await foreach (var bestellung in auftragseingang.GetBestellungen(30))
    {
      Print($"Bestellung {bestellung.Auftragsnummer} mit {bestellung.Artikel.Count()} Artikeln nach {bestellung.Adresse.Ort}", ConsoleColor.Yellow);

      try
      {
        await channelAuftragseingang.Writer.WriteAsync(bestellung, cancellationToken);

      }
      catch (TaskCanceledException)
      {
        Print("BestellungenVerarbeiten abgebrochen", ConsoleColor.Red);
      }
    }

    channelAuftragseingang.Writer.Complete();
  }

  static async Task Verpacken(Bestellung bestellung)
  {
    Print($"Verpackung {bestellung.Auftragsnummer} begonnen", ConsoleColor.Magenta);
    await Task.Delay(bestellung.Artikel.Sum(a => a.Verpackungszeit) * 100);
    Print($"Verpackung {bestellung.Auftragsnummer} beendet, jetzt versenden", ConsoleColor.Magenta);
    if (bestellung.Adresse.Versandzeit > 1)
    {
      await channelVersandAusland.Writer.WriteAsync(bestellung, cancellationToken);
      Print($"Auftrag {bestellung.Auftragsnummer} ins Ausland versendet", ConsoleColor.Blue);
    }
    else
    {
      await channelVersandInland.Writer.WriteAsync(bestellung, cancellationToken);
      Print($"Auftrag {bestellung.Auftragsnummer} ins Inland versendet", ConsoleColor.Blue);
    }
  }

  static object syncObj = new();
  static void Print(string text, ConsoleColor color)
  {
    lock (syncObj)
    {
      var previousColor = Console.ForegroundColor;
      Console.ForegroundColor = color;
      Console.WriteLine($"{stopwatch.ElapsedMilliseconds:00,000} {text}");
      Console.ForegroundColor = previousColor;

    }
  }

}