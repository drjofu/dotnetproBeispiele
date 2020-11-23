using HFCommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFConsoleServer2.Services
{
  public class ServiceB : IServiceB
  {
    public Task DoSomethingElse(string todo)
    {
      Console.WriteLine("*** Server 2 - Service B: " + todo);
      return Task.CompletedTask;
    }
  }
}
