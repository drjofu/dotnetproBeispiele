using Hangfire;
using HFCommonLib;
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
  public class TaskEnqueueController : ControllerBase
  {
    [HttpPost]
    public void Post()
    {
      int ms = DateTime.Now.Millisecond;

      string id = "Job " + ms;
      int count = ms % 10;
      BackgroundJob.Enqueue<IServiceA>((s) => s.DoSomething(id, count));
    }

    [HttpPost("ServiceB")]
    public void PostServiceB()
    {
      BackgroundJob.Enqueue<IServiceB>(s => s.DoSomethingElse($"Instanz von ServiceB, eingereiht um {DateTime.Now.ToLongTimeString()}"));
    }

    [HttpPost("ServiceC")]
    public void PostServiceC()
    {
      BackgroundJob.Enqueue<IServiceC>(s => s.SendEmail(new EmailDetails("somebody@somewhere.com","Inhalt der E-Mail")));
    }

    [HttpPost("basic")]
    public void PostBasicService()
    {
      var d = new JobData("Just for fun", DateTime.Now);
      BackgroundJob.Enqueue(() => Console.WriteLine($"Todo at {d.timestamp:hh:mm} - {d.text}"));

      // {"Type":"System.Console, System.Console, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a","Method":"WriteLine","ParameterTypes":"[\"System.String, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\"]","Arguments":"[\"\\\"Todo at 06:43 - Just for fun\\\"\"]"}
      // ["\"Todo at 06:43 - Just for fun\""]
    }
    // {"Type":"HFCommonLib.IServiceA, HFCommonLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null","Method":"DoSomething","ParameterTypes":"[\"System.String, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\",\"System.Int32, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\"]","Arguments":"[\"\\\"Job 921\\\"\",\"1\"]"}


  }

  public record JobData(string text,DateTime timestamp);
}
