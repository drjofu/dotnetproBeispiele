using Microsoft.Extensions.Logging;

namespace Logausgaben;

public static partial class LoggingExtensions
{
  [LoggerMessage("Bearbeitung von Person {name}, Alter: {age} abgeschlossen", EventId = 99)]
  public static partial void LogPrecompiled(this ILogger logger, string name, int age, LogLevel logLevel = LogLevel.Warning);

  [LoggerMessage(LogLevel.Critical)]
  public static partial void LogExceptionPrecompiled(this ILogger logger, Exception ex);

  //[LoggerMessage(LogLevel.Critical)]
  //public static partial void LogMultipleExceptionsPrecompiled(this ILogger logger, Exception ex1, Exception ex2);

  [LoggerMessage("Bearbeitung von Person {person} gestartet", Level = LogLevel.Information)]
  public static partial void LogPerson(this ILogger logger, [LogProperties]Person person);

  [LoggerMessage("Logging a key/value-list", Level = LogLevel.Information)]
  public static partial void LogKeyValueList(this ILogger logger, Dictionary<string,object> myData);

  [LoggerMessage("Logging a list", Level = LogLevel.Information)]
  public static partial void LogList(this ILogger logger, IEnumerable<string> myList);

  [LoggerMessage("Bearbeitung 2 von Person beendet", Level = LogLevel.Information)]
  public static partial void LogPerson2(this ILogger logger, [TagProvider(typeof(LoggingExtensions), nameof(GetPersonLogData))] Person person);


  public static void GetPersonLogData(ITagCollector collector, Person person)
  {
    collector.Add("Name", person.Name);
    collector.Add("HomeAddress", person.Address);
  }

}
