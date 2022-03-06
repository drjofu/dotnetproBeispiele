namespace WinUI3MVVM.Models;

/// <summary>
/// Information of a single news item
/// </summary>
public class NewsItem
{
    public string Title { get; set; }
    public string Summary { get; set; }
    public DateTimeOffset PublishDate { get; set; }
    public string ImageUrl { get; set; }
    public string WebsiteUrl { get; set; }

    public int Rating { get; set; }
    public string Category { get; set; }

}
