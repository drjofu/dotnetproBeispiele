using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUI3MVVM.Models;

/// <summary>
/// Abstraction of shell commands that are implemented in MainWindow.xaml.cs
/// </summary>
public interface IShell
{
    void NavigateTo(Type pageType, object args);
    void NavigateTo<TView>() where TView : Page;

    void SetupAudio(bool isAudioOn, bool isAudioSpacial);
    (bool isAudioOn, bool isAudioSpacial) GetAudio();
    bool? LightThemeSelected { get; set; }
    string WindowTitle { get; set; }
    Task<ContentDialogResult> ShowDialog<TDialogView>(DialogSettings settings) where TDialogView : FrameworkElement, new();

    string GetResourceString(string resourceId);
    bool SetResourceCulture(string cultureId);

}
