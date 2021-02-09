using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Utilities
{
  /// <summary>
  /// Hilfsstruktur zur Bindung von Rückrufmethoden für Drag & Drop
  /// </summary>
  public class DragDropController
  {
    /// <summary>
    /// Übergabe des zu ziehenden TreeItems, Rückgabe ob erlaubt oder nicht
    /// </summary>
    public Func<TreeItem,bool> CanDrag { get; set; }

    /// <summary>
    /// 1. Parameter: das gezogene Objekt
    /// 2. Parameter: das TreeItem unter der Maus
    /// 3. Parameter: Prozentwert der Mausposition in Bezug auf das TreeItem in Parameter 2. 0: oben, 100: unten
    /// Rückgabe: Erlaubte Drag&Drop-Effekte
    /// </summary>
    public Func<object, TreeItem, double, DragDropEffects> CanDrop { get; set; }

    /// <summary>
    /// Abschluss der Drag&Drop-Operation
    /// 1. Parameter: das gezogene Objekt
    /// 2. Parameter: das TreeItem unter der Maus
    /// 3. Parameter: Prozentwert der Mausposition in Bezug auf das TreeItem in Parameter 2. 0: oben, 100: unten
    /// Rückgabe: wird derzeit nicht berücksichtigt
    /// </summary>
    public Func<object, TreeItem, double, bool> Drop { get; set; }
  }
}
