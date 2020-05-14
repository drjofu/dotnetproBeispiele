using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGlobalisierung.GlobalizationUtilities
{
  public class SelectableItem
  {
    public event EventHandler IsSelectedChanged;

    private bool isSelected;

    public bool IsSelected
    {
      get { return isSelected; }
      set
      {
        if (isSelected != value)
        {
          isSelected = value;
          if (IsSelectedChanged != null) IsSelectedChanged(this, EventArgs.Empty);
        }
      }
    }

  }
}
