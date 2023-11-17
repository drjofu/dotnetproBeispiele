using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logausgaben
{
  public class Person
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    [LogPropertyIgnore]
    public string Address { get; set; }

    public override string ToString()
    {
      return $"Person: {Name}";
    }
  }
}
