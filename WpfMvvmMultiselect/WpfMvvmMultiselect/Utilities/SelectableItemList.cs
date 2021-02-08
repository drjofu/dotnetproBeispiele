using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WpfMvvmMultiselect.Utilities
{
  /// <summary>
  /// Liste von SelectableItem
  /// Hilfsklasse zum Erstellen der Auflistung und Nachverfolgen der ausgewählten Elemente
  /// </summary>
  public class SelectableItemList : ObservableCollection<SelectableItem>
  {
    /// <summary>
    /// Factory-Methode zum Erstellen der Auflistung
    /// </summary>
    /// <param name="items">Liste der Datenobjekte</param>
    /// <returns>Liste der SelectableItems</returns>
    public static SelectableItemList FromItems(IEnumerable items)
    {
      var list = new SelectableItemList();
      foreach (var item in items)
      {
        list.Add(new SelectableItem { Data = item });
      }
      return list;
    }

    private  void Si_IsSelectedChanged(object sender, EventArgs e)
    {
      base.OnPropertyChanged( new PropertyChangedEventArgs(nameof(SelectedItems)));
    }

    protected override void InsertItem(int index, SelectableItem item)
    {
      base.InsertItem(index, item);
      // Auf Änderungen von IsSelected reagieren
      item.IsSelectedChanged += Si_IsSelectedChanged;
    }

    protected override void RemoveItem(int index)
    {
      // Handler wieder entfernen
      this[index].IsSelectedChanged += Si_IsSelectedChanged;
      base.RemoveItem(index);
    }

    /// <summary>
    /// Ausgewählte Elemente
    /// </summary>
    public IEnumerable<SelectableItem> SelectedItems => this.Where(i => i.IsSelected).ToList();
  }
}
