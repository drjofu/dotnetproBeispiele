using System;

namespace DependencyInjectionBeispiel
{
  class Program
  {
    static void Main(string[] args)
    {
      // DI-Container einrichten
      ServiceLocator.Current.AddSingleton<S>(new S());
      ServiceLocator.Current.AddTransient<IA, A>();
      ServiceLocator.Current.AddTransient<IB, B>();
      ServiceLocator.Current.AddTransient<IC, C>();
      ServiceLocator.Current.AddSingleton<D>();

      // Objekte abrufen
      var s = ServiceLocator.Current.GetInstance<S>();
      Console.WriteLine(s.Name);

      var a = ServiceLocator.Current.GetInstance<IA>();
      a.TuWas();

      var b = ServiceLocator.Current.GetInstance<IB>();
      b.MachWas();

      var c = ServiceLocator.Current.GetInstance<IC>();
      c.Ausgabe();

      var d1 = ServiceLocator.Current.GetInstance<D>();
      var d2 = ServiceLocator.Current.GetInstance<D>();
      Console.WriteLine(d1==d2);

      Console.ReadLine();
    }
  }
}
