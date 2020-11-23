using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HFApiClient.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class JobInfoController : ControllerBase
  {
    [HttpGet]
    public string Get()
    {
      var api = JobStorage.Current.GetMonitoringApi();
      var servers = api.Servers();
      var text = $"\"Servers: {servers.Count}\"";
      var jobs = api.EnqueuedJobs("default", 0, 10);
      var n = jobs.Count();
      return text;
    }
  }
}
