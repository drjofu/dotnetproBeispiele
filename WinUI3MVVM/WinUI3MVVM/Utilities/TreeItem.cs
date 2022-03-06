using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace WinUI3MVVM.Utilities;

/// <summary>
/// Utility class for defining tree structures
/// </summary>
/// <typeparam name="DataType"></typeparam>
public class TreeItem<DataType> : NotificationObject
{
    private string title;

    /// <summary>
    /// Display text
    /// </summary>
    public string Title
    {
        get { return title; }
        set { title = value; OnPropertyChanged(); }
    }

    private IconElement icon;

    // Display icon
    public IconElement Icon
    {
        get { return icon; }
        set { icon = value; OnPropertyChanged(); }
    }

    private DataType data;

    /// <summary>
    /// Connected data
    /// </summary>
    public DataType Data
    {
        get { return data; }
        set { data = value; OnPropertyChanged(); }
    }

    private ObservableCollection<TreeItem<DataType>> children = new();

    /// <summary>
    /// Child items
    /// </summary>
    public ObservableCollection<TreeItem<DataType>> Children
    {
        get { return children; }
        set { children = value; }
    }

    private bool isSelected;

    /// <summary>
    /// Property to connect to IsSelected property of control (e. g. via Style)
    /// </summary>
    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            if (isSelected == value) return;
            isSelected = value;
            OnPropertyChanged();
        }
    }


}
