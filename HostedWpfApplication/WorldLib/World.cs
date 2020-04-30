using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace WorldLib
{
  public class World
  {
    private readonly string path;

    public World(string path)
    {
      this.path = path;
    }

    public IEnumerable<Continent> GetContinents()
    {
      return XDocument.Load(path).Root.Elements("continent")
        .Select(xContinent => new Continent
        {
          Name = xContinent.Element("name").Value,
          Area = (int)xContinent.Element("area"),
          ShortName = xContinent.Attribute("id").Value
        });
    }

    public IEnumerable<Country> GetCountriesOnContinent(string continentShortName)
    {
      var xRoot = XDocument.Load(path).Root;
      var xContinent = xRoot.Elements("continent").FirstOrDefault(xContinent => xContinent.Attribute("id").Value == continentShortName);
      if (xContinent is null) return null;

      var countries = xRoot.Elements("country")
                  .Where(xCountry => xCountry.Element("encompassed").Attribute("continent").Value == continentShortName)
          .Select(xCountry => new Country
          {
            Name = xCountry.Element("name").Value,
            CarCode = xCountry.Attribute("car_code").Value,
            Population = (int)xCountry.Element("population"),
            Area = (double)xCountry.Attribute("area"),
            Government = (string)xCountry.Element("government"),
            IndependenceDate = (DateTime?)xCountry.Element("indep_date")
          })
          .ToList();
      return countries;
    }
  }
}
