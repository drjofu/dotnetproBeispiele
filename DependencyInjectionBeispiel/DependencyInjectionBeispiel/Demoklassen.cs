using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionBeispiel
{
  public class S
  {
    public string Name { get; set; } = "ein S";
  }

  public interface IA
  {
    void TuWas();
  }

  public class A : IA
  {
    public void TuWas()
    {
      Console.WriteLine("TuWas A");
    }
  }

  public interface IB
  {
    void MachWas();
  }

  public class B : IB
  {
    private readonly IA a;
    private readonly S s;

    public B(IA a, S s)
    {
      this.a = a;
      this.s = s;
    }
    public void MachWas()
    {
      Console.WriteLine("B Machwas mit " + s.Name);
      a.TuWas();
    }
  }

  public interface IC { void Ausgabe(); }
  public class C : IC
  {
    private readonly IB b;

    public C(IB b)
    {
      this.b = b;
    }
    public void Ausgabe()
    {
      Console.WriteLine("Ausgabe C");
      b.MachWas();
    }
  }

  public class D
  {
    public D(IB b)
    {
      Console.WriteLine("ctor D");
      b.MachWas();
    }
  }
}
