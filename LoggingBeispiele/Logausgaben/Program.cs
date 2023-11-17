using Logausgaben;
using Microsoft.Extensions.DependencyInjection;
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

//Exception ex = new ApplicationException("ohh...");
//var text = "Information 1";
//var zahl = 123;
//logger.Log(LogLevel.Information, 42, ex, "Auszugebende Informationen: {info1}, {info2}", text, zahl);


//logger.LogInformation("Hallo Welt");

var name = "Peter Pan";

// string concatination
logger.LogInformation("Bearbeitung von " + name + " gestartet");

// klassisches string.Format
logger.LogInformation(string.Format("Bearbeitung von {0} gestartet", name));

// string interpolation (ab C# 6)
logger.LogInformation($"Bearbeitung von {name} gestartet");

// Log-Template mit Parameter(n)
logger.LogInformation("Bearbeitung von {name} gestartet", name);

if (logger.IsEnabled(LogLevel.Information))
  logger.LogInformation("...");

LogMessages.LogNameAndAge(logger, "Peter Pan", 27, null);
logger.LogNameAndAgeHelper("Peter Pan", 99);

logger.LogPrecompiled("Peter Pan", 39, LogLevel.Error);

var exception = new ApplicationException("I did not expect this to happen...");
logger.LogExceptionPrecompiled(exception);

logger.Log<LogInformation>(
  LogLevel.Warning,
  123, // EventId
  new LogInformation
  {
    Name = "Donald Duck",
    Age = 76
  },
  null,
  (li, _) => $"Bearbeitung von {li.Name}, Alter: {li.Age} abgeschlossen");




struct LogInformation
{
  public int Age { get; set; }
  public string Name { get; set; }
}

static class LogMessages
{
  // Predefine Log-Message
  public static readonly Action<ILogger, string, int, Exception?> LogNameAndAge =
    LoggerMessage.Define<string, int>(
      logLevel: LogLevel.Information,
      eventId: 12,
      formatString: "Bearbeitung von Person {name}, Alter: {age} abgeschlossen");

  // Helper as extension method
  public static void LogNameAndAgeHelper(this ILogger logger, string name, int age)
  {
    LogNameAndAge(logger, name, age, null);
  }
}