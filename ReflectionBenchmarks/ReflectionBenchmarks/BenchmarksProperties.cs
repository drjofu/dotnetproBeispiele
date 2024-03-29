﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionBenchmarks
{
  [MemoryDiagnoser()]
  public class BenchmarksProperties
  {
    private readonly object obj = new Beispielklasse();
    private readonly Beispielklasse obj2 = new Beispielklasse();
    private dynamic dBeispielklasse;

    private PropertyInfo propertyInfo1, propertyInfo2;
    private PropertyDescriptor propertyDescriptor1, propertyDescriptor2;

    public BenchmarksProperties()
    {
      obj2 = new Beispielklasse();
      obj = obj2;
      dBeispielklasse=obj;

      var type = typeof(Beispielklasse);
      propertyInfo1 = type.GetProperty("Name1")!;
      propertyInfo2 = type.GetProperty("Name2")!;

      var pds = System.ComponentModel.TypeDescriptor.GetProperties(obj);
      propertyDescriptor1 = pds.Find("Name1", true)!;
      propertyDescriptor2 = pds.Find("Name2", true)!;

    }

    [Benchmark(Baseline = true, Description = "Property compiled")]
    public string ExchangeNamesCompiled()
    {
      string t = obj2.Name1;
      obj2.Name1 = obj2.Name2;
      obj2.Name2 = t;
      return t;
    }

    [Benchmark(Description = "Property via PropertyInfo")]
    public string ExchangeNamesReflection()
    {
      object? v;
      v = propertyInfo1.GetValue(obj);
      propertyInfo1.SetValue(obj, propertyInfo2.GetValue(obj));
      propertyInfo2.SetValue(obj, v);
      return (string)v!;
    }

    [Benchmark( Description = "Property via dynamic")]
    public string ExchangeNamesDynamic()
    {
      string t = dBeispielklasse.Name1;
      dBeispielklasse.Name1 = dBeispielklasse.Name2;
      dBeispielklasse.Name2 = t;
      return t;
    }

    [Benchmark(Description = "Property via PropertyDescriptor")]
    public string ExchangeNamesReflectionX()
    {
      object? v;
      v = propertyDescriptor1.GetValue(obj);
      propertyDescriptor1.SetValue(obj, propertyDescriptor2.GetValue(obj));
      propertyDescriptor2.SetValue(obj, v);
      return (string)v!;
    }


    [GlobalCleanup]
    public void GlobalCleanup()
    {
      // Zur Kontrolle
      Console.WriteLine($"Counts: {Beispielklasse.Counter1} / {Beispielklasse.Counter2}");
    }

  }
}
