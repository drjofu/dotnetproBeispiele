using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using System.Xml;

namespace WinUI3MVVM.Models;

[Export(AsSingleton = true)]
public class RssFeedReader : NotificationObject
{
  const string alternateImageUrl = "https://reisen.fuechse-online.de/assets/journeys/Alpen2012/images/img_9617.jpg";
  private readonly IShell shell;

  //public string Url { get; set; } = "http://www.spiegel.de/schlagzeilen/tops/index.rss";
  //    public string Url { get; set; } = "http://www.heise.de/newsticker/heise-atom.xml";
  //  public string Url { get; set; } = "http://feeds.hanselman.com/_/20/572565368/scotthanselman";
  public string Url { get; set; } = "https://www.n-tv.de/rss";

  public RssFeedReader(IShell shell)
  {
    this.shell = shell;
  }

  private NewsItem selectedItem;

  // Selected news item
  public NewsItem SelectedItem
  {
    get { return selectedItem; }
    set
    {
      if (selectedItem == value) return;
      selectedItem = value;
      shell.WindowTitle = "RSS - " + selectedItem?.Title;
      OnPropertyChanged();
    }
  }

  private NewsFeed newsFeed;

  // Data of news feed
  public NewsFeed NewsFeed
  {
    get { return newsFeed; }
    set { newsFeed = value; OnPropertyChanged(); }
  }

  private IEnumerable<IGrouping<string, NewsItem>> categories;

  // List of news, grouped by categories
  public IEnumerable<IGrouping<string, NewsItem>> Categories
  {
    get { return categories; }
    set { categories = value; OnPropertyChanged(); }
  }


  private bool isBusy = false;

  // Rss feed reader is busy loading data
  public bool IsBusy
  {
    get { return isBusy; }
    set { isBusy = value; OnPropertyChanged(); }
  }

  private bool isLoaded;

  // All data loaded
  public bool IsLoaded
  {
    get { return isLoaded; }
    set { isLoaded = value; OnPropertyChanged(); }
  }


  // Asynchronously load data from url
  public async Task ReadFeed()
  {
    IsBusy = true;
    IsLoaded = false;
    await Task.Delay(1);

    // load feed
    var reader = XmlReader.Create(Url);
    var feed = await Task.Run(() => SyndicationFeed.Load(reader));

    // Create data structure
    var newsFeed = new NewsFeed()
    {
      Title = feed.Title.Text,
      Description = feed.Description?.Text,
      ImageUrl = feed.ImageUrl?.AbsoluteUri ?? alternateImageUrl,
      LastUpdatedTime = feed.LastUpdatedTime,
      Url = Url,
    };
    newsFeed.Items = new ObservableCollection<NewsItem>();
    foreach (var item in feed.Items)
    {
      try
      {
        newsFeed.Items.Add(new NewsItem
        {
          Title = item.Title?.Text,
          ImageUrl = item.Links.Count > 1 ? item.Links[1].Uri.AbsoluteUri : alternateImageUrl,
          PublishDate = item.PublishDate,
          Summary = item.Summary?.Text,
          Category = item.Categories.FirstOrDefault()?.Name,
          WebsiteUrl = item.Links[0].Uri?.AbsoluteUri

        });

      }
      catch (Exception)
      {
      }
    }

    Categories = newsFeed.Items
      .GroupBy(ni => ni.Category);

    NewsFeed = newsFeed;

    SelectedItem = NewsFeed.Items.FirstOrDefault();
    IsBusy = false;
    IsLoaded = true;
  }
}
