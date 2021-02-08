using System;

namespace WpfMvvmMultiselect.Utilities
{
  /// <summary>
  /// Listenelement für die Bindung an ListBox, DataGrid etc.
  /// </summary>
  public class SelectableItem : NotificationObject
  {
    /// <summary>
    /// Wird ausgelöst, wenn IsEnabled geändert wurde
    /// </summary>
    public event EventHandler IsEnabledChanged;

    /// <summary>
    /// Wird ausgelöst, wenn IsSelected geändert wurde
    /// </summary>
    public event EventHandler IsSelectedChanged;

    private object data;

    /// <summary>
    /// Angehängtes Datenobjekt
    /// </summary>
    public object Data
    {
      get { return data; }
      set { data = value; OnPropertyChanged(); }
    }

    private bool isEnabled = true;

    /// <summary>
    /// Gibt an, ob das Element verfügbar ist
    /// </summary>
    public bool IsEnabled
    {
      get { return isEnabled; }
      set
      {
        if (isEnabled == value) return;
        isEnabled = value;
        OnPropertyChanged();
        IsEnabledChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    private bool isSelected;

    /// <summary>
    /// Gibt an, ob das Element ausgewählt wurde
    /// </summary>
    public bool IsSelected
    {
      get { return isSelected; }
      set
      {
        if (IsSelected == value) return;
        isSelected = value;
        OnPropertyChanged();
        IsSelectedChanged?.Invoke(this, EventArgs.Empty);
      }
    }


  }
}
