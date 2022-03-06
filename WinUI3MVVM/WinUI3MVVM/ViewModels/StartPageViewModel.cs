namespace WinUI3MVVM.ViewModels;

/// <summary>
/// ViewModel of start page view
/// </summary>
public class StartPageViewModel : ViewModelBase
{
    public StartPageViewModel(IShell shell) : base(shell)
    {
        //Title = "Startseite";
        Title = shell.GetResourceString("StartPage"); // get title from resource
    }

    private string welcomeMessage = "Willkommen";

    // Message to show as content
    public string WelcomeMessage
    {
        get { return welcomeMessage; }
        set
        {
            if (welcomeMessage == value) return;
            welcomeMessage = value;
            OnPropertyChanged();
        }
    }

}
