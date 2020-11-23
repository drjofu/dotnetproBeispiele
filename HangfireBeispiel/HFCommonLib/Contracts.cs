using Hangfire;
using System;
using System.Threading.Tasks;

namespace HFCommonLib
{
  public interface IServiceA
  {
    Task DoSomething(string id, int count);
  }

  [Queue("queue1")]
  public interface IServiceB
  {
    Task DoSomethingElse(string todo);
  }

  [Queue("queue2")]
  public interface IServiceC
  {
    Task SendEmail(EmailDetails emailDetails);
  }

  public record EmailDetails(string To, string Body);
}
