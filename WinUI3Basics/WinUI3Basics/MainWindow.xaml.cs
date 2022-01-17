using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Reflection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3Basics
{
  /// <summary>
  /// An empty window that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainWindow : Window
  {
    public MainWindow()
    {
      this.InitializeComponent();
    }

    private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
      // Text aus Tag lesen
      string typename = args.InvokedItemContainer.Tag as string;

      // Typ-Objekt ermitteln
      typename = $"{nameof(WinUI3Basics)}.{typename}";
      var type = Assembly.GetExecutingAssembly().GetType(typename);

      // Navigation anstoßen. Die Instanz der Page wird automatisch über Reflection erzeugt
      contentFrame.Navigate(type);
    }
  }
}
