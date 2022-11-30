using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FunWithAwaitables
{
  // https://www.youtube.com/watch?v=ileC_qyLdD4&list=WL&index=1

  public class MagicDelay
  {
    private readonly TimeSpan timeSpan;
    public TimeSpan TimeSpan => timeSpan;

    private MagicDelay(TimeSpan timeSpan)
    {
      this.timeSpan = timeSpan;
    }

    public static MagicDelay Milliseconds(int milliseconds)
    {
      return new MagicDelay(TimeSpan.FromMilliseconds(milliseconds));
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.taskawaiter?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Runtime.CompilerServices.TaskAwaiter)%3Bk(DevLang-csharp)%26rd%3Dtrue&view=net-6.0
    // TaskAwaiter: This type is intended for compiler use only.

    //public TaskAwaiter GetAwaiter()
    //{
    //  return Task.Delay(timeSpan).GetAwaiter();
    //}
  }


  public static class MagicExtensions
  {
    public static TaskAwaiter GetAwaiter(this MagicDelay magicDelay)
    {
      return Task.Delay(magicDelay.TimeSpan).GetAwaiter();
    }

    public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
    {
      return Task.Delay(timeSpan).GetAwaiter();
    }

    public static TaskAwaiter GetAwaiter(this int milliseconds)
    {
      return Task.Delay(TimeSpan.FromMilliseconds(milliseconds)).GetAwaiter();
    }


  }
}
