using Hangfire;
using Hangfire.PostgreSql;
using HFCommonLib;
using HFConsoleServer2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace HFConsoleServer2
{
  class Program
  {
    private static BackgroundJobServer backgroundJobServer;
    static async Task Main(string[] args)
    {
      Console.WriteLine("************************** Console-Service **************************");
      var host = await CreateHostBuilder(args).StartAsync();

      // Hangfire einrichten
      var connstr = host.Services.GetService<IConfiguration>().GetConnectionString("HangfireConnection");
      GlobalConfiguration.Configuration.UsePostgreSqlStorage(connstr);
      GlobalConfiguration.Configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
      GlobalConfiguration.Configuration.UseSimpleAssemblyNameTypeSerializer();
      GlobalConfiguration.Configuration.UseRecommendedSerializerSettings();
      GlobalConfiguration.Configuration.UseColouredConsoleLogProvider();

      // Activator für .NET-Dependency-Injection einsetzen
      GlobalConfiguration.Configuration.UseActivator(new DIJobActivator(host.Services));

      // Benachrichtigungen einrichten
      //GlobalConfiguration.Configuration.UseFilter(new OnPerformedCallbackFilter(null));

      // Einstellen, aus welchen Queues Aufgaben entnommen werden sollen
      backgroundJobServer = new BackgroundJobServer(new BackgroundJobServerOptions { Queues=new[] { "queue1", "default" } });

      Console.ReadLine();
      Console.WriteLine("----------------------- Console-Service Ende -------");
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
      var builder = Host.CreateDefaultBuilder(args);

      builder.ConfigureServices((hostContext, services) =>
      {
        // Service-Klassen über DI verknüpfen
        services.AddTransient<IServiceA, ServiceA>();
        services.AddTransient<IServiceB, ServiceB>();
      });
      
      return builder;
    }
  }

  // Hilfsklasse für die Verwendung der .NET-Dependency-Injection
  public class DIJobActivator : JobActivator
  {
    private readonly IServiceProvider services;

    public DIJobActivator(IServiceProvider services)
    {
      // Service-Provider speichern
      this.services = services;
    }
    public override object ActivateJob(Type jobType)
    {
      // Objekt für vorgegebenen Typ von Service-Provider holen
      return services.GetService(jobType);
    }
  }
}
