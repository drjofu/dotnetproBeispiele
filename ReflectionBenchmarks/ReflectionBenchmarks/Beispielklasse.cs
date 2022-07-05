using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionBenchmarks
{
  public class Beispielklasse
  {
    public Beispielklasse()
    {
    }

    public static long Counter1 { get; set; }
    public static long Counter2 { get; set; }

    private string name1 = "Marie";
    private string name2 = "Peter";

    public string Name1 { get => name1; set { name1 = value; Counter1++; } }
    public string Name2 { get => name2; set { name2 = value; Counter2++; } }

    public int Add(int a, int b) { return a + b; }
  }
}
