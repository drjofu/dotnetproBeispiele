using CommonTypes;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Timers;

namespace Services.Models
{
  public class Tankstellenverwaltung
  {
    private List<Tankstelle> tankstellen;

    private Random rnd = new Random();

    private Timer timer;

    /// <summary>
    /// Observable informiert über Änderungen der Tankstellenpreise
    /// </summary>
    public ReplaySubject<List<Tankstelle>> Tankstellen { get; set; } = new(3);

    // Hub wird über Dependency Injection bereit gestellt
    public Tankstellenverwaltung()
    {
      // Tankstellenliste und Timer initialisieren
      tankstellen = new List<Tankstelle>
      {
        new Tankstelle{Name="Aral", Diesel=1.33, Super=1.55},
        new Tankstelle{Name="Shell", Diesel=1.32, Super=1.53},
        new Tankstelle{Name="Esso", Diesel=1.35, Super=1.56}
      };

      timer = new Timer(2000);
      timer.Elapsed += Timer_Elapsed;
      timer.Start();
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      // aktuelle Tankstellenpreise ermitteln
      var diesel = Math.Round(Math.Max(0.95, Math.Min(1.5, rnd.Next(-20, 20) * 0.001 + tankstellen[0].Diesel)), 2);
      var super = Math.Round(Math.Max(1.2, Math.Min(1.8, rnd.Next(-20, 20) * 0.001 + tankstellen[0].Super)), 2);
      foreach (var t in tankstellen)
      {
        t.Diesel = Math.Round(diesel + (rnd.NextDouble() - 0.5) * 0.1, 2);
        t.Super = Math.Round(super + (rnd.NextDouble() - 0.5) * 0.1, 2);
      }

      // Aktuelle Preise über ReactiveExtensions an registrierte Clients senden
      Tankstellen.OnNext(tankstellen);

    }

   
  }
}
