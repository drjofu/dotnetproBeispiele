using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WpfAsyncBasics.ViewModels;

namespace WpfAsyncBeispiele
{
  public class Beispiel3ViewModel:ViewModelBase
  {
    private int progressInfo;

    public int ProgressInfo
    {
      get { return progressInfo; }
      set { progressInfo = value; OnPropertyChanged(); }
    }

    private string result;

    public string Result
    {
      get { return result; }
      set { result = value; OnPropertyChanged(); }
    }

    private AsyncTaskModel model = new AsyncTaskModel();

    public AsyncActionCommand<int> CountdownAsyncCommand { get; set; }

    public Beispiel3ViewModel()
    {
      Title = "Async Command";
      CountdownAsyncCommand = new AsyncActionCommand<int>(RunCountdownAsync, ShowProgress, CompletedHandler);
    }

    private void CompletedHandler(TaskStatus state, Exception ex)
    {
      if (state == TaskStatus.Canceled)
        Result = "Countdown abgebrochen";
      else if (state == TaskStatus.Faulted)
        Result = "Rakete explodiert! Grund: " + ex.Message;
      else
        Console.WriteLine("ok");
    }


    private void ShowProgress(int progress)
    {
      ProgressInfo = progress;
    }

    private async Task RunCountdownAsync(CancellationToken token, IProgress<int> showProgress)
    {
      Result = "Countdown läuft";
      Result = await model.CountdownAsync(10, token, showProgress);
    }
  }


}
