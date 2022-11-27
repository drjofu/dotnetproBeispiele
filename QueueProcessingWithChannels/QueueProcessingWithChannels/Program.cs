using QueueProcessingWithChannels;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Channels;

public class Program
{
  // zur Zeitmessung
  static readonly Stopwatch stopwatch = Stopwatch.StartNew();

  // Channel für die Auftragsbearbeitung (Verpacken)
  static readonly Channel<Bestellung> channelAuftragseingang = Channel.CreateUnbounded<Bestellung>();

  // Channel für den Versand
  static readonly Channel<Bestellung> channelVersandInland = Channel.CreateBounded<Bestellung>(new BoundedChannelOptions(5) { SingleReader = true });
  static readonly Channel<Bestellung> channelVersandAusland = Channel.CreateBounded<Bestellung>(2);

  // Beispieldatengenerator
  static readonly Auftragseingang auftragseingang = new();

  // Token (-generator) für den Abbruch durch den Benutzer
  static CancellationTokenSource cancellationTokenSource = new();
  static CancellationToken cancellationToken = cancellationTokenSource.Token;

  public static async Task Main()
  {
    //BeispielMitConcurrentQueue();
    await BeispielMitChannels();

  }

  // Konventioneller Ansatz mit ConcurrentQueue und Semaphore
  private static void BeispielMitConcurrentQueue()
  {
    ConcurrentQueue<Bestellung> queue = new();
    SemaphoreSlim semaphoreSlim = new(0);

    Task.Run(async () =>
    {
      Print("Bestellungen entgegennehmen", ConsoleColor.Yellow);
      await foreach (var bestellung in auftragseingang.GetBestellungen(10))
      {
        Print($"Auftrag {bestellung.Auftragsnummer} in queue verschoben", ConsoleColor.Yellow);
        queue.Enqueue(bestellung);
        semaphoreSlim.Release();
      }
      Print("Alle Bestellungen entgegengenommen", ConsoleColor.Yellow);
    });

    Task.Run(async() =>
    {
      Print("Bearbeitung der Bestellungen begonnen", ConsoleColor.Blue);

      while (true)
      {
        await semaphoreSlim.WaitAsync();
        if(queue.TryDequeue(out var bestellung))
        {
          Print($"Beginn Auftrag {bestellung.Auftragsnummer} verpacken", ConsoleColor.Cyan);
          await Task.Delay(bestellung.Artikel.Sum(a => a.Verpackungszeit) * 100, cancellationToken);
          Print($"Ende Auftrag {bestellung.Auftragsnummer} verpacken", ConsoleColor.Cyan);
        }
      }

      //Print("Bearbeitung der Bestellungen beendet", ConsoleColor.Blue);
    });


    Console.ReadLine();
  }

  // Ansatz mit Channels
  private static async Task BeispielMitChannels()
  {
    AbbruchDurchBenutzerÜberwachen();

    var tasks = new List<Task>();
    var tasksVerpackungsteam = new List<Task>();

    tasks.Add(BestellungenVerarbeiten());

    tasksVerpackungsteam.Add(VerpackungsteamStarten("A"));
    tasksVerpackungsteam.Add(VerpackungsteamStarten("B"));
    tasksVerpackungsteam.Add(VerpackungsteamStarten("C"));

    tasks.Add(PaketversandAusführen("Inland", channelVersandInland));
    tasks.Add(PaketversandAusführen("Ausland", channelVersandAusland));

    await Task.WhenAll(tasksVerpackungsteam);
    channelVersandAusland.Writer.Complete();
    channelVersandInland.Writer.Complete();

    await Task.WhenAll(tasks);

    if (cancellationToken.IsCancellationRequested)
      Print("Vorzeitiger Abbruch", ConsoleColor.Red);
    else
      Print("Alles erledigt", ConsoleColor.Green);
  }


  static async Task BestellungenVerarbeiten()
  {
    Print("Bestellungen entgegennehmen", ConsoleColor.Yellow);
    await foreach (var bestellung in auftragseingang.GetBestellungen(5, cancellationToken))
    {
      Print($"Bestellung {bestellung.Auftragsnummer} mit {bestellung.Artikel.Count()} Artikeln ({string.Join(",",bestellung.Artikel.Select(a=>a.Bezeichnung))}) nach {bestellung.Adresse.Ort}", ConsoleColor.Yellow);

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
    Print("Alle Bestellungen entgegengenommen", ConsoleColor.Yellow);

  }

  static async Task VerpackungsteamStarten(string name)
  {
    Print($"Verpackungsteam {name} hat Arbeit begonnen", ConsoleColor.Green);
    try
    {
      await foreach (var bestellung in channelAuftragseingang.Reader.ReadAllAsync(cancellationToken))
      {
        await Verpacken(name, bestellung);
      }

      Print($"Verpackungsteam {name} ist fertig", ConsoleColor.Green);

    }
    catch (OperationCanceledException)
    {
      Print($"Verpackungsteam {name} hat Arbeit abgebrochen", ConsoleColor.Red);
    }
  }

  static async Task Verpacken(string teamname, Bestellung bestellung)
  {
    Print($"Verpackung von Auftrag {bestellung.Auftragsnummer} durch {teamname} begonnen", ConsoleColor.Magenta);
    await Task.Delay(bestellung.Artikel.Sum(a => a.Verpackungszeit) * 100, cancellationToken);
    Print($"Verpackung von Auftrag {bestellung.Auftragsnummer} durch {teamname} beendet, jetzt versenden", ConsoleColor.Magenta);
 
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

  private static async Task PaketversandAusführen(string name, Channel<Bestellung> channelVersand)
  {
    try
    {
      Print($"Logistiker ({name}) bereit zum Versenden der Pakete", ConsoleColor.Cyan);
      await foreach (var item in channelVersand.Reader.ReadAllAsync(cancellationToken))
      {
        Print($"Versand für Auftrag {item.Auftragsnummer} nach {item.Adresse.Ort} begonnen. Dauer [Tage]: {item.Adresse.Versandzeit}", ConsoleColor.Cyan);
        await Task.Delay(item.Adresse.Versandzeit * 1000, cancellationToken);
        Print($"Ware von Auftrag {item.Auftragsnummer} in {item.Adresse.Ort} ausgeliefert", ConsoleColor.Cyan);
      }
      Print($"Logistiker ({name}) hat alle Versandaufträge abgeschlossen", ConsoleColor.Cyan);
    }
    catch (OperationCanceledException)
    {
      Print("Logistiker ({name}) hat Arbeit abgebrochen", ConsoleColor.Red);
    }
  }


  static readonly object syncObj = new();
  static void Print(string text, ConsoleColor color=ConsoleColor.White)
  {
    lock (syncObj)
    {
      var previousColor = Console.ForegroundColor;
      Console.ForegroundColor = color;
      Console.WriteLine($"{stopwatch.ElapsedMilliseconds:00,000} {text}");
      Console.ForegroundColor = previousColor;

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

}