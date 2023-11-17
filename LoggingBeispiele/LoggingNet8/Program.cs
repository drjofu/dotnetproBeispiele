using Logausgaben;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

var app = Host.CreateDefaultBuilder().Build();
using ILoggerFactory loggerFactory = LoggerFactory.Create(
    builder =>
    builder.AddJsonConsole(
        options =>
        options.JsonWriterOptions = new JsonWriterOptions()
        {
          Indented = true
        }));

//var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("Beispiel-Logger");

Person person = new() { Name = "Micky Mouse", Age = 55, Id = 177, Address = "Mousetown" };
Dictionary<string, object> data = new Dictionary<string, object> { ["info"] = "nur zur Information", ["date"] = DateTime.Now, ["person"] = person };

logger.LogPerson(person);

// https://learn.microsoft.com/de-de/dotnet/api/microsoft.extensions.logging.loggermessagehelper.stringify?view=dotnet-plat-ext-8.0

logger.LogKeyValueList(data);

logger.LogList(["eins", "zwei", "drei"]);


logger.LogPerson2(person);
