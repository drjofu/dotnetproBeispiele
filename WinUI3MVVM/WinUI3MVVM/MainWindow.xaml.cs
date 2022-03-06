// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUI3MVVM;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window, IShell
{
  private SettingsProvider settingsProvider;

  // resource management
  //private ResourceLoader resourceLoader = new();
  private ResourceManager resourceManager = new();
  private ResourceContext resourceContext;

  // ViewModel for main window
  public MainViewModel ViewModel { get; set; }

  public MainWindow()
  {
    this.InitializeComponent();
  }


  internal void Setup(IServiceProvider services)
  {
    settingsProvider = services.GetService<SettingsProvider>();


    // setup app and window settings
    SetupAudio();

    // setup culture settings
    resourceContext = resourceManager.CreateResourceContext();
    resourceContext.QualifierValues["Language"] = CultureInfo.CurrentCulture.Name; //"de-DE";

    var cultureId = (string)settingsProvider["CultureId"];

    if (cultureId == null) cultureId = CultureInfo.CurrentCulture.Name;
    else
    {
      CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureId);
      CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(cultureId);
    }

    resourceContext.QualifierValues["Language"] = cultureId;

    // für World-Model
    // Pfad anpassen!!!
    //if (!localSettings.Values.ContainsKey("pathMondial"))
    //  localSettings.Values["pathMondial"] = @"E:\Projekte\Schulungen\Beispiele-Schulungen\WinUI3\FuWinUI3Samples\FuWinUI3Samples\FuWinUI3Samples\Data\mondial.xml";

  }



  #region IShell (public available members)

  /// <summary>
  /// Navigate to page
  /// </summary>
  /// <param name="pageType">Type of desired page</param>
  /// <param name="args"></param>
  public void NavigateTo(Type pageType, object args)
  {
    contentFrame.Navigate(pageType, args);
  }

  /// <summary>
  /// Navigate to page
  /// </summary>
  /// <typeparam name="TView">Type of desired page</typeparam>
  public void NavigateTo<TView>() where TView : Page
  {
    contentFrame.Navigate(typeof(TView), typeof(TView).Name);
  }

  /// <summary>
  /// Setup app audio
  /// </summary>
  /// <param name="isAudioOn"></param>
  /// <param name="isAudioSpacial"></param>
  public void SetupAudio(bool isAudioOn, bool isAudioSpacial)
  {
    if (isAudioOn)
      ElementSoundPlayer.State = ElementSoundPlayerState.On;
    else
      ElementSoundPlayer.State = ElementSoundPlayerState.Off;

    if (isAudioSpacial && isAudioOn)
      ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
    else
      ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;

    settingsProvider["SoundPlayerState"] = ElementSoundPlayer.State.ToString();
    settingsProvider["SoundPlayerSpatial"] = ElementSoundPlayer.SpatialAudioMode.ToString();
  }

  /// <summary>
  /// Get current audio settings
  /// </summary>
  /// <returns></returns>
  public (bool isAudioOn, bool isAudioSpacial) GetAudio()
  {
    return (ElementSoundPlayer.State == ElementSoundPlayerState.On, ElementSoundPlayer.SpatialAudioMode == ElementSpatialAudioMode.On);
  }

  /// <summary>
  /// Set or get LightTheme / DarkTheme (null for auto)
  /// </summary>
  public bool? LightThemeSelected
  {
    get
    {
      if (nvMain.RequestedTheme == Microsoft.UI.Xaml.ElementTheme.Light) return true;
      if (nvMain.RequestedTheme == Microsoft.UI.Xaml.ElementTheme.Dark) return false;
      return null;
    }
    set
    {
      if (value != null)
        if (value.Value)
          nvMain.RequestedTheme = Microsoft.UI.Xaml.ElementTheme.Light;
        else
          nvMain.RequestedTheme = Microsoft.UI.Xaml.ElementTheme.Dark;
    }
  }

  /// <summary>
  /// Get or set title of the window (title bar)
  /// </summary>
  public string WindowTitle { get => this.Title; set => this.Title = value; }

  /// <summary>
  /// Show a view as a content dialog
  /// </summary>
  /// <typeparam name="TDialogView">type of the view</typeparam>
  /// <param name="settings">settings for buttons etc.</param>
  /// <returns>user choice</returns>
  public async Task<ContentDialogResult> ShowDialog<TDialogView>(DialogSettings settings) where TDialogView : FrameworkElement, new()
  {
    ContentDialog dialog = new ContentDialog()
    {
      Title = settings.Title,
      PrimaryButtonText = settings.PrimaryButtonText,
      CloseButtonText = settings.CloseButtonText,
      SecondaryButtonText = settings.SecondaryButtonText,
      DefaultButton = settings.DefaultButton,
      Width=2000,Height=1000 // does not work
    };

    dialog.XamlRoot = this.Content.XamlRoot;
    dialog.Content = new TDialogView()
    {
      MinWidth = 1000,
      MinHeight = 800 // does not work
    };
    var result = await dialog.ShowAsync(); // show as modal window
    return result;

  }

  /// <summary>
  /// Get string from resource for current culture
  /// </summary>
  /// <param name="resourceId"></param>
  /// <returns></returns>
  public string GetResourceString(string resourceId)
  {
    var resourceCandidate = resourceManager.MainResourceMap.TryGetValue($"Resources/{resourceId}", resourceContext);
    var resourceString = resourceCandidate?.ValueAsString;
    return resourceString;
  }

  /// <summary>
  /// Select resource culture
  /// </summary>
  /// <param name="cultureId">Culture name (e.g. de-DE, en-US etc.)</param>
  /// <returns></returns>
  public bool SetResourceCulture(string cultureId)
  {
    resourceContext.QualifierValues["Language"] = cultureId;
    //localSettings.Values["CultureId"] = cultureId;
    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureId);
    CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(cultureId);
    settingsProvider["CultureId"] = cultureId;
    return true;
  }


  #endregion

  // user clicked back arror button
  private void BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
  {
    if (contentFrame.CanGoBack)
      contentFrame.GoBack();
  }

  // navigation to different page is requested. Can be cancelled via e.Cancel
  private void OnRootFrameNavigating(object sender, NavigatingCancelEventArgs e)
  {
    var vm = ((sender as Frame)?.Content as Page)?.DataContext as ViewModelBase;
    if (vm != null)
    {
      vm.OnNavigatingFrom(e);
    }

  }

  // navigation to view completed
  private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
  {
    var vm = ((sender as Frame)?.Content as Page)?.DataContext as ViewModelBase;
    if (vm == null)
      nvMain.Header = "no ViewModel";
    else
      nvMain.Header = vm.Title;

  }

  // setup audio settings
  private void SetupAudio()
  {
    string settingsValue = (string)settingsProvider["SoundPlayerState"];
    if (settingsValue != null)
      ElementSoundPlayer.State = Enum.Parse<ElementSoundPlayerState>(settingsValue);
    settingsValue = (string)settingsProvider["SoundPlayerSpatial"];
    if (settingsValue != null)
      ElementSoundPlayer.SpatialAudioMode = Enum.Parse<ElementSpatialAudioMode>(settingsValue);
  }

}
