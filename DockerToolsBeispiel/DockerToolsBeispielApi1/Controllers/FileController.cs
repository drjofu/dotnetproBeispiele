using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using io = System.IO;

namespace DockerToolsBeispielApi1.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class FileController : ControllerBase
  {
    private const string path = "/var/lib/data/";

    [HttpGet]
    public async Task<string> Get(string filename)
    {
      Console.WriteLine($"Datei {filename} abgerufen");
      return await io.File.ReadAllTextAsync($"{path}{filename}.txt");
    }

    [HttpPost]
    public async Task Post(string filename, [FromBody] string content)
    {
      if (!io.Directory.Exists(path))
        io.Directory.CreateDirectory(path);

      await io.File.WriteAllTextAsync($"{path}{filename}.txt", content);
    }
  }
}
