using BenchmarkDotNet.Attributes;
using System.Reflection;

namespace ReflectionBenchmarks
{
  [MemoryDiagnoser()]
  public class BenchmarksCtor
  {
    [Benchmark(Baseline =true,Description ="compiled")]
    public Beispielklasse GetBeispielklasse_Direct()
    {
      return new Beispielklasse();
    }

    [Benchmark(Description ="via full reflection")]
    public Beispielklasse GetBeispielklasse_FullReflection()
    {
      var type=typeof(Beispielklasse);
      var ci = type.GetConstructor(new Type[] { });
      return (Beispielklasse) ci!.Invoke(new object[] { });
    }

    private Type type = typeof(Beispielklasse);
    private ConstructorInfo constructorInfo = typeof(Beispielklasse).GetConstructor(new Type[] { })!;

    [Benchmark(Description = "via optimized reflection")]
    public Beispielklasse GetBeispielklasse_OptimizedReflection()
    {
      return (Beispielklasse)constructorInfo.Invoke(new object[] { });
    }

    [Benchmark(Description = "via Activator.CreateInstance")]
    public Beispielklasse GetBeispielklasse_ActivatorCreate()
    {
      return (Beispielklasse)Activator.CreateInstance(type)!;
    }

  }
}
