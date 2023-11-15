﻿using Microsoft.Extensions.Logging;

namespace Logausgaben;

public static partial class LoggingExtensions
{
  [LoggerMessage("Bearbeitung von Person {name}, Alter: {age} abgeschlossen", EventId = 99)]
  public static partial void LogPrecompiled(this ILogger logger, string name, int age, LogLevel logLevel = LogLevel.Warning);

  [LoggerMessage(LogLevel.Critical)]
  public static partial void LogExceptionPrecompiled(this ILogger logger, Exception ex);

  //[LoggerMessage(LogLevel.Critical)]
  //public static partial void LogMultipleExceptionsPrecompiled(this ILogger logger, Exception ex1, Exception ex2);

}
