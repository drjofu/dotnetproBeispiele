namespace StructuredLogging
{
  public class Artikel
  {
    public int Nummer { get; set; }
    public required string Bezeichnung { get; set; }
    public double Preis { get; set; }

    public List<string>? Herstellerhinweise { get; set; }
  }
}
