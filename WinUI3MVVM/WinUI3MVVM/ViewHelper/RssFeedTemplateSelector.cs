
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUI3MVVM.ViewHelper;

/// <summary>
/// Select one of two applied DataTemplates depending on the type to be shown
/// </summary>
public class RssFeedTemplateSelector : DataTemplateSelector
{
  public DataTemplate CategoryTemplate { get; set; }
  public DataTemplate NewsItemTemplate { get; set; }

  protected override DataTemplate SelectTemplateCore(object item)
  {
    if (item is NewsItem)
      return NewsItemTemplate;
    return CategoryTemplate;
  }
}
