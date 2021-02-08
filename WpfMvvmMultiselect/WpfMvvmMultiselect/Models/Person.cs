using System.Collections.Generic;

namespace WpfMvvmMultiselect.Models
{
  class Person
  {
    public string Vorname { get; set; }
    public string Nachname { get; set; }
    public int Alter { get; set; }
  }

  class Personenliste : List<Person>
  {
    public Personenliste()
    {
      this.Add(new Person { Nachname = "Meier", Vorname = "Peter", Alter = 55 });
      this.Add(new Person { Nachname = "Meier", Vorname = "Petra", Alter = 44 });
      this.Add(new Person { Nachname = "Schmitz", Vorname = "Eva", Alter = 33 });
      this.Add(new Person { Nachname = "Schmidt", Vorname = "Uwe", Alter = 45 });
      this.Add(new Person { Nachname = "Schulze", Vorname = "Klaus", Alter = 56 });
    }
  }
}
