using BenchmarkDotNet.Attributes;

namespace BoxingBeiIEnumerable
{
  [MemoryDiagnoser]
  public class IEnumerableBenchmarkTests
  {
    public List<int> Zahlen { get; set; }

    public IEnumerableBenchmarkTests()
    {
      Zahlen = Enumerable.Range(0, 100).ToList();
    }

    [Benchmark(Baseline = true, Description = "foreach via List")]
    public int SummeListForEach()
    {
      int summe = 0;
      foreach (var zahl in Zahlen)
      {
        summe += zahl;
      }
      return summe;
    }

    [Benchmark(Description = "Schleife via List.GetEnumerator")]
    public int SummeListGetEnumerator()
    {
      int summe = 0;
      var enumerator = Zahlen.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          var zahl = enumerator.Current;
          summe += zahl;
        }
      }
      finally
      {
        enumerator.Dispose();
      }
      return summe;
    }

    [Benchmark(Description = "foreach via IEnumerable")]
    public int SummeIEnumerableForEach()
    {
      int summe = 0;
      IEnumerable<int> enumerable = Zahlen;
      foreach (var zahl in enumerable)
      {
        summe += zahl;
      }
      return summe;
    }

    [Benchmark(Description = "Schleife via IEnumerable.GetEnumerator")]
    public int SummeIEnumerableGetEnumerator()
    {
      int summe = 0;
      IEnumerable<int> enumerable = Zahlen;
      var enumerator = enumerable.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          var zahl = enumerator.Current;
          summe += zahl;
        }
      }
      finally
      {
        enumerator?.Dispose();
      }
      return summe;
    }

    [Benchmark(Description = "Summe via List/Linq")]
    public int SummeListLinq()
    {
      return Zahlen.Sum();
    }

    [Benchmark(Description = "Summe via IEnumerable/Linq")]
    public int SummeIEnumerableLinq()
    {
      IEnumerable<int> enumerable = Zahlen;
      return enumerable.Sum();
    }

    [Benchmark(Description = "for-Schleife mit Indizierung")]
    public int SummeFor()
    {
      int summe = 0;
      int n = Zahlen.Count;
      for (int i = 0; i < n; i++)
        summe += Zahlen[i];

      return summe;
    }


  }
}
