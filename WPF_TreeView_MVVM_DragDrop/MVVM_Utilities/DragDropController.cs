using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Utilities
{
  /// <summary>
  /// Ermitteln, ob ein TreeItem gezogen werden darf
  /// </summary>
  /// <param name="dragSource">das zu ziehende TreeItem</param>
  /// <returns>true, wenn ja</returns>
  public delegate bool CanDragHandler(TreeItem dragSource);

  /// <summary>
  /// Ermitteln, ob ein Objekt auf einem TreeItem abgelegt werden darf
  /// </summary>
  /// <param name="draggedItem">das gezogene Objekt</param>
  /// <param name="dropTarged">das TreeItem unter der Maus</param>
  /// <param name="yPosition">Prozentwert der Mausposition in Bezug auf das TreeItem in Parameter 2. 0: oben, 100: unten</param>
  /// <returns>Erlaubte Drag&Drop-Effekte</returns>
  public delegate DragDropEffects CanDropHandler(object draggedItem, TreeItem dropTarged, double yPosition);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="draggedItem">das gezogene Objekt</param>
  /// <param name="dropTarget">das TreeItem unter der Maus</param>
  /// <param name="yPosition">Prozentwert der Mausposition in Bezug auf das TreeItem in Parameter 2. 0: oben, 100: unten</param>
  /// <returns>Aktion war erfolgreich</returns>
  public delegate bool DropHandler(object draggedItem, TreeItem dropTarget, double yPosition);

  /// <summary>
  /// Hilfsstruktur zur Bindung von Rückrufmethoden für Drag & Drop
  /// </summary>
  public class DragDropController
  {
    /// <summary>
    /// Prüfen, ob ein Element gezogen werden darf
    /// </summary>
    public CanDragHandler CanDrag { get; set; }

    /// <summary>
    /// Prüfen, ob ein Element abgelegt werden darf
    /// </summary>
    public CanDropHandler CanDrop { get; set; }

    /// <summary>
    /// Schlussbehandlung der Drag&Drop-Aktion
    /// </summary>
    public Func<object, TreeItem, double, bool> Drop { get; set; }
  }
}
