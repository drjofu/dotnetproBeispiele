using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Utilities
{
  /// <summary>
  /// Hilfsklasse für TreeView-Datenstruktur
  /// </summary>
  public class TreeItem : NotificationObject
  {
    public TreeItem()
    {
      Items = new TreeItemList(this);
    }

    private object data;

    /// <summary>
    /// Angehängtes Model-Objekt
    /// </summary>
    public object Data
    {
      get { return data; }
      set { data = value; OnPropertyChanged(); }
    }

    private bool isSelected;

    /// <summary>
    /// Repräsentation der IsSelected-Eigenschaft des TreeViewItems
    /// </summary>
    public bool IsSelected
    {
      get { return isSelected; }
      set
      {
        if (isSelected == value) return;
        isSelected = value; 
        OnPropertyChanged();
        IsSelectedChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Wird gefeuert, wenn sich IsSelected ändert
    /// </summary>
    public event EventHandler IsSelectedChanged;

    private bool isExpanded;

    /// <summary>
    /// Repräsentation der IsExpanded-Eigenschaft des TreeViewItems
    /// </summary>
    public bool IsExpanded
    {
      get { return isExpanded; }
      set { isExpanded = value; OnPropertyChanged(); }
    }

    private bool isEnabled = true;

    public bool IsEnabled
    {
      get { return isEnabled; }
      set { isEnabled = value; OnPropertyChanged(); }
    }


    /// <summary>
    /// Untergeordnete Items
    /// </summary>
    public TreeItemList Items { get; set; }

    /// <summary>
    /// Parent dieses Items
    /// </summary>
    public TreeItem Parent { get; set; }

    /// <summary>
    /// Daten für Anzeige eines Tooltips
    /// </summary>
    public object ToolTip { get; set; }


    public void ExpandAncestors(bool expand)
    {
      if (Parent != null)
      {
        Parent.IsExpanded = expand;
        Parent.ExpandAncestors(expand);
      }
    }


  }
}
