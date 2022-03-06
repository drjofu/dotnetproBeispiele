using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace WinUI3MVVM.ViewModels;

/// <summary>
/// ViewModel of the main window
/// </summary>
[Export(AsSingleton = true)]
public class MainViewModel : ViewModelBase
{
  // Menu tree for navigation
  public ObservableCollection<PageTypeTreeItem> MenuItems { get; set; }

  public MainViewModel(IShell shell) : base(shell)
  {
    // initialize the menu tree
    MenuItems = new()
    {
      new PageTypeTreeItem { Title = shell.GetResourceString("StartPage"), Icon = new SymbolIcon(Symbol.Home), Data = typeof(StartPageView) },
      new PageTypeTreeItem
      {
        Title = "RSS-Feed",
        Icon = new SymbolIcon(Symbol.Globe),
        Children = new()
        {
          new PageTypeTreeItem { Title = "Classic", Icon = new SymbolIcon(Symbol.List), Data = typeof(RssFeedReaderClassicView) },
          new PageTypeTreeItem { Title = "FlipView", Icon = new SymbolIcon(Symbol.List), Data = typeof(RssFeedReaderFlipView) },
          new PageTypeTreeItem { Title = "GridView", Icon = new SymbolIcon(Symbol.SlideShow), Data = typeof(RssFeedReaderGridView) },
          new PageTypeTreeItem { Title = "ListView", Icon = new SymbolIcon(Symbol.View), Data = typeof(RssFeedReaderListView) },
          new PageTypeTreeItem { Title = "TreeView", Icon = new SymbolIcon(Symbol.Street), Data = typeof(RssFeedReaderTreeView) },
          new PageTypeTreeItem { Title = "SemanticZoom", Icon = new SymbolIcon(Symbol.ZoomIn), Data = typeof(RssFeedReaderSemanticZoomView) }
        }
      },

      new PageTypeTreeItem
      {
        Title = "Beispiele",
        Children = new()
        {
          new PageTypeTreeItem { Title = "Beispiel 1", Icon = new SymbolIcon(Symbol.Scan), Data = typeof(Sample1View) },
          new PageTypeTreeItem { Title = "Beispiel 2", Icon = new SymbolIcon(Symbol.SelectAll), Data = typeof(Sample2View) }
        }
      }
    };

    // select start page
    shell.NavigateTo(typeof(StartPageView), "");
  }

  // user clicked on menu item
  public void MenuItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs e)
  {
    // click on settings icon?
    if (e.IsSettingsInvoked)
    {
      shell.NavigateTo(typeof(SettingsView), "settings");
      return;
    }

    // else get the desired view type
    var treeItem = e.InvokedItemContainer.Tag as PageTypeTreeItem;
    if (treeItem?.Data != null)
      shell.NavigateTo(treeItem.Data, treeItem.Title);

  }


}
