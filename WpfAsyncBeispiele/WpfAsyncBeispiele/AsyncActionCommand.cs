using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAsyncBeispiele
{
  // siehe auch: https://johnthiriet.com/mvvm-going-async-with-async-command/#

  public class AsyncActionCommand<TProgress> : ICommand, INotifyPropertyChanged
  {
    private readonly Func<CancellationToken, IProgress<TProgress>, Task> asyncAction;
    private readonly IProgress<TProgress> showProgress;
    private readonly Action<TaskStatus, Exception> completedHandler;
    private CancellationTokenSource cts;
    private bool isBusy = false;

    private bool isEnabled = true;

    private Task runningTask = null;

    public bool IsEnabled
    {
      get { return isEnabled; }
      set { isEnabled = value; NotifyCanExecuteChanged(); }
    }

    private TaskStatus lastExecutionState;

    public TaskStatus LastExecutionState
    {
      get { return lastExecutionState; }
      protected set { lastExecutionState = value; OnPropertyChanged(); }
    }

    public ActionCommand CancelCommand { get; }


    public AsyncActionCommand(Func<CancellationToken, IProgress<TProgress>, Task> asyncAction, Action<TProgress> progressHandler = null, Action<TaskStatus, Exception> completedHandler = null)
    {
      this.asyncAction = asyncAction;
      this.showProgress = new Progress<TProgress>(progressHandler);
      this.completedHandler = completedHandler;
      this.CancelCommand = new ActionCommand(Cancel) { IsEnabled = false };
    }

    private void Cancel()
    {
      cts.Cancel();
    }

    public event EventHandler CanExecuteChanged;
    public event PropertyChangedEventHandler PropertyChanged;

    public bool CanExecute(object parameter)
    {
      return isEnabled && !isBusy;
    }

    public async void Execute(object parameter)
    {
      cts?.Dispose();
      cts = new CancellationTokenSource();
      isBusy = true; NotifyCanExecuteChanged();
      CancelCommand.IsEnabled = true;

      try
      {
        runningTask = Task.Run(() =>
          asyncAction(cts.Token, showProgress), cts.Token);

        LastExecutionState = TaskStatus.Running;

        await runningTask;

        LastExecutionState = TaskStatus.RanToCompletion;
        completedHandler?.Invoke(LastExecutionState, null);
      }
      catch (Exception ex)
      {
        LastExecutionState = runningTask.Status;
        completedHandler?.Invoke(LastExecutionState, ex);
      }

      isBusy = false;
      NotifyCanExecuteChanged();
      CancelCommand.IsEnabled = false;
    }

    protected void NotifyCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

  }
}


