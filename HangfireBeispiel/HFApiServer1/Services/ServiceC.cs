using HFCommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HFApiServer1.Services
{
  public class ServiceC : IServiceC
  {
    public async Task SendEmail(EmailDetails emailDetails)
    {
      //throw new ApplicationException("da ist was schiefgegangen");

      Console.WriteLine($"*** Server 1 - Sending EMail to {emailDetails.To}, content: {emailDetails.Body}");
      await Task.Delay(5000);
      Console.WriteLine("*** Server 1 - EMail sent to " + emailDetails.To);
    }
  }
}
