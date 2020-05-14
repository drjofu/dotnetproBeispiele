using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMUtilities
{
  /// <summary>
  /// Alternative zur {Binding}-MarkupExtension. NotifyOnValidationError wird hier auf true gesetzt, sonst bleibt alles gleich
  /// </summary>
  public class Binding : System.Windows.Data.Binding
  {
    public Binding()
    {
      this.NotifyOnValidationError = true;
    }

    public Binding(string path):base(path)
    {
      this.NotifyOnValidationError = true;
    }
  }
}
