using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDebugVisualizer
{
  

  public class Planet
  {
    public override string ToString()
    {
      return $"{name} - radius: {radius.value}{radius.units}";
    }

    public string color { get; set; }
    public string name { get; set; }
    public IEnumerable<Satellite> satellites { get; set; }=new List<Satellite>();

    public Distance distance { get; set; }
    public Radius radius { get; set; }
    public double length_of_year { get; set; }
    public Day day { get; set; }
    public Mass mass { get; set; }
    public Density density { get; set; }
  }

  public class Distance
  {
    public string units { get; set; }
    public string value { get; set; }
  }

  public class Radius
  {
    public string units { get; set; }
    public string value { get; set; }
  }

  public class Day
  {
    public string units { get; set; }
    public string value { get; set; }
  }

  public class Mass
  {
    public string units { get; set; }
    public string value { get; set; }
  }

  public class Density
  {
    public string units { get; set; }
    public string value { get; set; }
  }

  public class Satellite
  {
    public string name { get; set; }
    public string distance_from_planet { get; set; }
    public Orbit orbit { get; set; }
  }

  public class Orbit
  {
    public string units { get; set; }
    public string value { get; set; }
  }

}
