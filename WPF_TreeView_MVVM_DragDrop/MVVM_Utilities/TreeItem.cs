﻿using System;
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
      set { isSelected = value; OnPropertyChanged(); }
    }

    private bool isExpanded;

    /// <summary>
    /// Repräsentation der IsExpanded-Eigenschaft des TreeViewItems
    /// </summary>
    public bool IsExpanded
    {
      get { return isExpanded; }
      set { isExpanded = value; OnPropertyChanged(); }
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


    public void ExpandToParent(bool expand)
    {
      if (Parent != null)
      {
        Parent.IsExpanded = expand;
        Parent.ExpandToParent(expand);
      }
    }


  }
}
