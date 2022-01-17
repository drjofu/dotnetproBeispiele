using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.GrpcServices;
using Services.Hubs;
using Services.Models;

namespace Services
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      // Tankstellenverwaltung als Singleton anlegen
      services.AddSingleton<Tankstellenverwaltung>();

      // Hosted Service, um �nderungen �ber SignalR bekannt zu machen
      services.AddHostedService<TankstellenSignalRHostedService>();

      // Dienste f�r SignalR bereitstellen
      services.AddSignalR();

      // Dienste f�r gRPC bereit stellen
      services.AddGrpc();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        // Endpunkt f�r SignalR-Service
        endpoints.MapHub<TankstellenHub>("/tankstellen");

        // Endpunkt f�r gRPC-Service
        endpoints.MapGrpcService<TankstellenGrpcService>();

        endpoints.MapGet("/", async context =>
              {
                await context.Response.WriteAsync("Hello World!");
              });
      });
    }
  }
}
