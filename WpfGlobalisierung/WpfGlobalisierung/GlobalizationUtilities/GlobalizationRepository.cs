using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WpfGlobalisierung.GlobalizationUtilities
{
  public class GlobalizationRepository
  {
    // Beispielimplementierung für in einer XML-Struktur abgelegte Texte
    public static string GetString(string cultureId, string id)
    {
      XDocument doc = XDocument.Load("data/texts.xml");
      var xCulture = doc.Root.Elements("Culture").FirstOrDefault(c => c.Attribute("id").Value == cultureId);
      if (xCulture == null) return "!!" + id + "!!";
      var xEntry = xCulture.Elements("Entry").FirstOrDefault(e => e.Attribute("id").Value == id);
      if (xEntry == null) return "!" + id + "!";
      return xEntry.Value;
    }
  }
}
