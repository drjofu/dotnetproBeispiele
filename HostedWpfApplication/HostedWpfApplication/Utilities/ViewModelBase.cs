using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace HostedWpfApplication.Utilities
{
  public class ViewModelBase : NotificationObject
  {
    public IConfiguration Configuration { get; }
    public ILogger Logger { get; }

    public string Title { get; set; }

    public ViewModelBase()
    {
      var services = App.AppHost.Services;
      Configuration = services.GetService<IConfiguration>();

      Type tlogger = typeof(ILogger<>);
      Type t = tlogger.MakeGenericType(this.GetType());
      Logger = (ILogger)services.GetService(t);
    }
  }
}
