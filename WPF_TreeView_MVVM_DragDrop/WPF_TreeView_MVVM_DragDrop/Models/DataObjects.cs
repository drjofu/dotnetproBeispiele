using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TreeView_MVVM_DragDrop.Models
{
  // Model-Klassen für Beispielprojekt
  public class DataObjectBase
  {
    public string Caption { get; set; }
    public int Level { get; set; }
  }

  public class DirectoryDataObject : DataObjectBase { }
  public class FileDataObject : DataObjectBase { }

}
