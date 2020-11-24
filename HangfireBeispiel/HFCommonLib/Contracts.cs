using Hangfire;
using System;
using System.Threading.Tasks;

namespace HFCommonLib
{
  [AutomaticRetry(Attempts =3)]
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
  [AutomaticRetry(Attempts = 3)]
  public interface IServiceC
  {
    Task SendEmail(EmailDetails emailDetails);
  }

  public record EmailDetails(string To, string Body);
}
