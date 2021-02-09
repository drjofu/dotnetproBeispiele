using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Utilities
{
  /// <summary>
  /// Liste von TreeItems, Änderungsbenachrichtigung über INotifyCollectionChanged
  /// </summary>
  public class TreeItemList : ObservableCollection<TreeItem>
  {
    private readonly TreeItem owner;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="owner">Das TreeItem, zu dem diese Liste gehört</param>
    public TreeItemList(TreeItem owner)
    {
      this.owner = owner;
    }

    /// <summary>
    /// Beim Hinzufügen eines neuen Items dessen Parent-Eigenschaft setzen
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    protected override void InsertItem(int index, TreeItem item)
    {
      base.InsertItem(index, item);
      item.Parent = owner;
    }

    /// <summary>
    /// Auf- oder Zuklappen aller Knoten dieser und aller darunter liegenden Ebenen
    /// </summary>
    /// <param name="expand">true: aufklappen, false: zuklappen</param>
    public void ExpandOrCollapseAll(bool expand)
    {
      foreach (var item in this)
      {
        item.IsExpanded = expand;
        item.Items.ExpandOrCollapseAll(expand);
      }
    }
  }
}
