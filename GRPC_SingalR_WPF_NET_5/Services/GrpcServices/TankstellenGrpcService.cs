using Grpc.Core;
using Microsoft.Extensions.Logging;
using Services.Models;
using Services.Protos;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Services.Protos.TankstellenGrpc;

namespace Services.GrpcServices
{
  /// <summary>
  /// Implementierung des gRPC-Services
  /// </summary>
  public class TankstellenGrpcService: TankstellenGrpcBase
  {
    // Singleton der Tankstellenverwaltung
    private readonly Tankstellenverwaltung tankstellenverwaltung;

    // Logger für diesen Dienst
    private readonly ILogger<TankstellenGrpcService> logger;

    // ctor für Dependency Injection
    public TankstellenGrpcService(Tankstellenverwaltung tankstellenverwaltung, ILogger<TankstellenGrpcService> logger)
    {
      this.tankstellenverwaltung = tankstellenverwaltung;
      this.logger = logger;
    }

    /// <summary>
    /// Die zu überschreibende Service-Methode
    /// </summary>
    /// <param name="request">Daten der Anfrage</param>
    /// <param name="responseStream">Der Stream, in den die Ausgabe erfolgen soll</param>
    /// <param name="context"></param>
    /// <returns>Fertigmeldung über Task-Objekt</returns>
    public override async Task Tankstellenpreise(TankstellenpreiseRequest request, IServerStreamWriter<Tankstellen> responseStream, ServerCallContext context)
    {
      // Awaitable anlegen, um die asynchrone Abarbeitung steuern zu können
      var taskCompletionSource = new TaskCompletionSource();

      // max. Anzahl der zu übertragenden Datensätze
      int count = request.Anzahl;

      // Subscription auf das Observable der Tankstellenverwaltung
      var subscription = tankstellenverwaltung.Tankstellen.Subscribe(async liste =>
      {
        try
        {
          // Prüfen, ob der Stream abgebrochen werden soll
          context.CancellationToken.ThrowIfCancellationRequested();

          // Von Protobuf generierte Datenklasse instanzieren und mit den erhaltenen Daten füllen
          var response = new Tankstellen();
          response.Liste.AddRange(liste.Select(t => new Tankstelle { Name = t.Name, Super = t.Super, Diesel = t.Diesel }));

          // Daten über Stream senden
          await responseStream.WriteAsync(response);

          count--;
          if (count <= 0)
          {
            // Wenn Anzahl erreicht, kann die Methode kontrolliert beendet werden
            taskCompletionSource.SetResult();
          }

        }
        catch (Exception ex)
        {
          logger.LogError(ex, "Abbruch Grpc-Stream");

          // Kontrollierter Abbruch der Methode
          taskCompletionSource.SetResult();
        }
      });

      // Auf die Fertigstellung der Aufgabe bzw. deren Abbruch warten
      await taskCompletionSource.Task;

      // Subscription beenden
      subscription.Dispose();

    }

  }
}
