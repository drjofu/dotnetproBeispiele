using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAsyncBeispiele
{
  public class AsyncTaskModel
  {

    public Task<string> IncrementAsync(CancellationToken token = default, IProgress<int> progress = null)
    {

      return Task<string>.Run(async () =>
      {
        for (int i = 0; i < 10; i++)
        {
          if (token.IsCancellationRequested)
          {
            // Aufräumen, falls nötig
            token.ThrowIfCancellationRequested();
          }
          await Task.Delay(1000);
          //throw new ApplicationException("oh nein...");

          progress?.Report(i);
        }

        return "das war schwierig...";

      }, token); // token muss übergeben werden, sonst ist bei Abbruch der Status des Task-Objekts "faulted"

    }

    public async Task<string> CountdownAsync(int count, CancellationToken token = default, IProgress<int> progress = null)
    {
      for (int i = count; i >= 0; i--)
      {
        token.ThrowIfCancellationRequested();

        await Task.Delay(1000);

        // if (i == 2) throw new ApplicationException("o-o");

        progress?.Report(i);
      }

      return "Rakete gestartet";
    }
  }
}
