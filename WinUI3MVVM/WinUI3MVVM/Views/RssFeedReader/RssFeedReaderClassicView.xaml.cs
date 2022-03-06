using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3MVVM.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RssFeedReaderClassicView : Page
{
    public RssFeedReaderViewModel ViewModel { get; set; }

    public RssFeedReaderClassicView()
    {
        this.InitializeComponent();
        ViewModel = this.SetupViewModel<RssFeedReaderViewModel>();
        ViewModel.Title = "RSS-Feed-Reader classic";
    }
}
