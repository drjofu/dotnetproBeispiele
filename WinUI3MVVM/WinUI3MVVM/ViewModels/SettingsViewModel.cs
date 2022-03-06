using System.Reflection;

namespace WinUI3MVVM.ViewModels;

/// <summary>
/// ViewModel for settings view
/// </summary>
[Export(AsSingleton = true)]
public class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel(IShell shell, SettingsProvider settingsProvider) : base(shell)
    {
        Title = "Settings";

        // get current settings
        (isAudioOn, isAudioSpacial) = shell.GetAudio();
        selectedCulture = CultureInfo.CurrentCulture;
        this.settingsProvider = settingsProvider;
    }

    private bool isAudioOn;

    /// <summary>
    /// Audio status
    /// </summary>
    public bool IsAudioOn
    {
        get { return isAudioOn; }
        set { isAudioOn = value; shell.SetupAudio(isAudioOn, isAudioSpacial); }
    }

    private bool isAudioSpacial;

    /// <summary>
    /// Audio spacial setting
    /// </summary>
    public bool IsAudioSpacial
    {
        get { return isAudioSpacial; }
        set { isAudioSpacial = value; shell.SetupAudio(isAudioOn, isAudioSpacial); }
    }

    /// <summary>
    /// Get app version
    /// </summary>
    public string Version
    {
        get
        {
            try
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            catch (System.Exception)
            {
                var asm = Assembly.GetExecutingAssembly();
                return asm.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            }
        }
    }

    /// <summary>
    /// Select or get current theme
    /// </summary>
    public bool? LightThemeSelected
    {
        get => shell.LightThemeSelected;
        set => shell.LightThemeSelected = value;
    }

    /// <summary>
    /// Cultures supported by this app. Static information for demo
    /// </summary>
    public List<CultureInfo> Cultures { get; set; } = new() { CultureInfo.GetCultureInfo("de-DE"), CultureInfo.GetCultureInfo("en-US") };

    private CultureInfo selectedCulture;
    private readonly SettingsProvider settingsProvider;

    /// <summary>
    /// Currently selected culture
    /// </summary>
    public CultureInfo SelectedCulture
    {
        get { return selectedCulture; }
        set
        {
            if (selectedCulture == value) return;
            selectedCulture = value;
            shell.SetResourceCulture(value.Name);
        }
    }

    /// <summary>
    /// For exercise: path to file mondial.xml
    /// </summary>
    public string PathMondial
    {
        get { return (string)settingsProvider["pathMondial"]; }
        set { settingsProvider["pathMondial"] = value; }
    }

}
