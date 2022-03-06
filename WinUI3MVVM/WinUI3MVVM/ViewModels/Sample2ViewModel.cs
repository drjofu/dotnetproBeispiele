namespace WinUI3MVVM.ViewModels;

/// <summary>
/// Example to play around 
/// </summary>
public class Sample2ViewModel : ViewModelBase
{
    public Sample2ViewModel(IShell shell, World world) : base(shell)
    {
        Title = "Sample 2 xxl";
        dispatcherTimer.Tick += DispatcherTimer_Tick;
        dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
        dispatcherTimer.Start();

        selectedDate = DateTimeOffset.Now.AddDays(10);
        World = world;
    }

    private void DispatcherTimer_Tick(object sender, object e)
    {
        Counter = (Counter + 1) % 101;
    }

    private DispatcherTimer dispatcherTimer = new();

    private int counter;

    public int Counter
    {
        get { return counter; }
        set
        {
            counter = value;
            OnPropertyChanged();
        }
    }

    private DateTimeOffset selectedDate;

    public DateTimeOffset SelectedDate
    {
        get { return selectedDate; }
        set
        {
            selectedDate = value;
            OnPropertyChanged();
        }
    }

    private TimeSpan selectedTime;

    public TimeSpan SelectedTime
    {
        get { return selectedTime; }
        set
        {
            if (selectedTime == value) return;
            selectedTime = value;
            OnPropertyChanged();
        }
    }


    public ObservableCollection<DateTimeOffset> SelectedDates { get; set; } = new() { DateTimeOffset.Now, DateTimeOffset.Now.AddDays(5) };
    public World World { get; }

    //public override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    //{
    //  e.Cancel = true;
    //}
}
