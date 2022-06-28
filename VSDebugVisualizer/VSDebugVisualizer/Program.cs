using System.Text.Json;
using VSDebugVisualizer;

string xml = File.ReadAllText("Planets.xml");
string json = File.ReadAllText("Planets.json");
string html = File.ReadAllText("info.html");

var planets = JsonSerializer.Deserialize<IEnumerable<Planet>>(json);




Console.WriteLine("Ende");