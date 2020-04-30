using System;

namespace WorldLib
{
  public class Country
  {
    public string Name { get; set; }
    public string CarCode { get; set; }
    public int Population { get; set; }

    public string Government { get; set; }
    public double Area { get; set; }
    public DateTime? IndependenceDate { get; set; }

  }
}
