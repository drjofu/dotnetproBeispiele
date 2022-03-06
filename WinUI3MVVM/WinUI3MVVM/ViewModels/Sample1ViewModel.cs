namespace WinUI3MVVM.ViewModels;

/// <summary>
/// Example to play around 
/// </summary>
public class Sample1ViewModel : ViewModelBase
{
    private readonly RssFeedReader rssFeedReader;

    public Sample1ViewModel(IShell shell, RssFeedReader rssFeedReader) : base(shell)
    {
        Title = "Sample 1";
        this.rssFeedReader = rssFeedReader;
    }

    public string Info { get; set; } = "Hallo von Sample1ViewModel";

    public async override void PageLoaded(object sender, RoutedEventArgs e)
    {
        base.PageLoaded(sender, e);
        await rssFeedReader.ReadFeed();

    }
}
