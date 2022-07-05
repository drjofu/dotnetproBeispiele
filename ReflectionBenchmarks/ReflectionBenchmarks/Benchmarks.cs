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
  public class Benchmarks
  {
    private readonly object obj = new Beispielklasse();
    private readonly Beispielklasse obj2 = new Beispielklasse();

    private Type type;
    private PropertyInfo propertyInfo1, propertyInfo2;
    private MethodInfo? methodInfo;

    public Benchmarks()
    {
      type = GetObjType();
      propertyInfo1 = type.GetProperty("Name1")!;
      propertyInfo2 = type.GetProperty("Name2")!;

      methodInfo = type.GetMethod("Add");
    }

    //[Benchmark]
    public string? GetName()
    {
      return (string?)obj.GetType()?.GetProperty("Name")?.GetValue(obj);
    }

    //[Benchmark]
    public Type GetObjType()
    {
      return obj.GetType();
    }

    //[Benchmark]
    public PropertyInfo GetPropertyInfo()
    {
      return type.GetProperty("Name1")!;
    }

    //[Benchmark(Description = "Property-Zugriff über Reflection (typed)")]
    //public string GetNameOptimized2()
    //{
    //  object? v;
    //  v = propertyInfo1.GetValue(obj2);
    //  propertyInfo1.SetValue(obj2, propertyInfo2.GetValue(obj2));
    //  propertyInfo2.SetValue(obj2, v);
    //  //return (string)v!;
    //  return "";
    //}


    [Benchmark(Baseline = true, Description = "Property-Zugriff direkt")]
    public string GetNameCompiled()
    {
      string t = "";
      t = obj2.Name1;
      obj2.Name1 = obj2.Name2;
      obj2.Name2 = t;
      return t;
    }

    [Benchmark(Description = "Property-Zugriff über Reflection")]
    public string GetNameOptimized()
    {
      object? v;
      v = propertyInfo1.GetValue(obj);
      propertyInfo1.SetValue(obj, propertyInfo2.GetValue(obj));
      propertyInfo2.SetValue(obj, v);
      return (string)v!;
    }


    //[Benchmark(Baseline = true)]
    public int AddCompiled()
    {
      return obj2.Add(200, 2);
    }

    // [Benchmark]
    public int AddReflection()
    {
      return (int)methodInfo!.Invoke(obj, new object[] { 11, 100 })!;
    }

    private readonly object[] numbers = { 1000, 123 };

    //[Benchmark]
    public object? AddReflectionConstantArray()
    {
      return methodInfo!.Invoke(obj, numbers);
    }

  }
}
