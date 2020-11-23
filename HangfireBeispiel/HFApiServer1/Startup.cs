using Hangfire;
using Hangfire.PostgreSql;
using HFApiServer1.Services;
using HFCommonLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HFApiServer1
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "HFApiServer1", Version = "v1" });
      });

      var connstr = Configuration.GetConnectionString("HangfireConnection");
      services.AddHangfire(config =>
            config
              .UsePostgreSqlStorage(connstr)
              .UseFilter(new OnPerformedCallbackFilter(this)));

      // Add the processing server as IHostedService
      services.AddHangfireServer(options=>
      {
        options.Queues = new[] { "queue2", "default" };
      });

      services.AddTransient<IServiceA, ServiceA>();
      services.AddTransient<IServiceC, ServiceC>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      //GlobalJobFilters.Filters.Add(new OnPerformedCallbackFilter());

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HFApiServer1 v1"));
      }

      app.UseRouting();

     // app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
 //       endpoints.MapHangfireDashboard(new DashboardOptions { Authorization = new[] { new HFAuthorizationFilter() } });

      });
    }
  }
}
