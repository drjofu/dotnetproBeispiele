using HFCommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HFApiServer1.Services
{
  public class ServiceA : IServiceA
  {
    public async Task DoSomething(string id, int count)
    {
      Console.WriteLine($"*** Server 1 -  {id} Service A started");
      for (int i = 0; i < count; i++)
      {
        await Task.Delay(1000);
        Console.WriteLine($"*** Server 1 -  {id} Service A: {i}");
      }
      Console.WriteLine($"*** Server 1 -  {id} Service A completed");
    }
  }
}
