using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Services.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Models
{

  /// <summary>
  /// Hosted Service zum Veröffentlichen geänderter Tankstellenpreise via SignalR
  /// </summary>
  public class TankstellenSignalRHostedService : BackgroundService
  {
    public TankstellenSignalRHostedService(Tankstellenverwaltung tankstellenverwaltung, IHubContext<TankstellenHub> tankstellenHub)
    {
      // Subscription auf Observable der Tankstellenliste
      tankstellenverwaltung.Tankstellen.Subscribe(t =>
      {
        // Änderungen an alle Clients schicken
        tankstellenHub.Clients.All.SendAsync("kraftstoffpreise", t);
      });
    }

    // muss wg. der Ableitung von BackgroundService überschrieben werden
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      return Task.CompletedTask;
    }
  }
}
