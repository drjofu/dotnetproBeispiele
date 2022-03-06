using System.Collections.ObjectModel;

namespace WinUI3MVVM.Models;

// Information about the RSS-feed including the list of news items
public class NewsFeed
{
    public string Url { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }

    public ObservableCollection<NewsItem> Items { get; set; }

}
