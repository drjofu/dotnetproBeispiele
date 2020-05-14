using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfAsyncBasics.ViewModels;

namespace WpfAsyncBeispiele
{
  public class ViewModelBeispiel2 : ViewModelBase
  {
    private AsyncTaskModel model;
    private CancellationTokenSource cts;

    private string info;

    public string Info
    {
      get { return info; }
      set { info = value; OnPropertyChanged(); }
    }

    private string result;

    public string Result
    {
      get { return result; }
      set { result = value; OnPropertyChanged(); }
    }


    public ActionCommand StartCommand { get; set; }
    public ActionCommand StopCommand { get; set; }


    public ViewModelBeispiel2()
    {
      Title = "Task, Cancel, Progress";
      StartCommand = new ActionCommand(Start);
      StopCommand = new ActionCommand(Stop) { IsEnabled = false };

      model = new AsyncTaskModel();
    }

    private void Stop()
    {
      cts?.Cancel();
    }

    private async void Start()
    {
      // Token-Quelle für eventuellen Abbruch holen
      cts = new CancellationTokenSource();

      // UI vorbereiten
      StartCommand.IsEnabled = false;
      StopCommand.IsEnabled = true;
      Result = "-";
      Info = "Started";

      Task<string> t = null;

      try
      {
        // Aktion starten
        t = model.IncrementAsync(cts.Token, new Progress<int>(ShowProgress));
        Result = await t;
      }
      catch (Exception ex) { }

      // Aktion beendet -> UI aktualisieren
      Info = t?.Status.ToString();
      StopCommand.IsEnabled = false;
      StartCommand.IsEnabled = true;
    }

    private void ShowProgress(int progress)
    {
      Info = "Progress: " + progress;
    }
  }
}
