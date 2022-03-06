using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3MVVM.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Sample1View : Page
{
  public Sample1ViewModel ViewModel { get; set; }

  public Sample1View()
  {
    this.InitializeComponent();
    ViewModel = this.SetupViewModel<Sample1ViewModel>();
  }
}
