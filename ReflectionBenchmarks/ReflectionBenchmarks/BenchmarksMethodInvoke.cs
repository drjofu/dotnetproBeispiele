using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionBenchmarks
{
  [MemoryDiagnoser()]
  public class BenchmarksMethodInvoke
  {
    private readonly object obj = new Beispielklasse();
    private readonly Beispielklasse obj2 = new Beispielklasse();

    private MethodInfo? methodInfo;

    private Random rnd = new Random();

    public BenchmarksMethodInvoke()
    {
      obj2 = new Beispielklasse();
      obj = obj2;

      var type = typeof(Beispielklasse);
      methodInfo = type.GetMethod("Add");
    }

    [Benchmark(Baseline = true, Description = "Method call compiled")]
    public int AddCompiled()
    {
      return obj2.Add(rnd.Next(), rnd.Next());
    }

    [Benchmark(Description = "Method call incl. parameter declaration")]
    public int AddReflection()
    {
      return (int)methodInfo!.Invoke(obj, new object[] { rnd.Next(), rnd.Next() })!;
    }

    private readonly object[] numbers = { 1000, 123 };

    [Benchmark(Description = "Method call with constant parameters")]
    public object? AddReflectionConstantArray()
    {
      return methodInfo!.Invoke(obj, numbers);
    }

  }
}
