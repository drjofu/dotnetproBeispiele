using Microsoft.Extensions.Logging;

namespace LoggingBeispiele
{
  public class InMemoryProvider : ILoggerProvider
  {
    public ILogger CreateLogger(string categoryName) => new InMemoryLogger();

    public void Dispose()
    {
    }

    private class InMemoryLogger : ILogger
    {
      private int _count;

      public IDisposable? BeginScope<TState>(TState state) where TState : notnull
      {
        return NullScope.Instance;
      }

      public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

      public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
      {
        _count++;
      }
    }

    private sealed class NullScope : IDisposable
    {
      public static NullScope Instance { get; } = new NullScope();

      /// 
      public void Dispose()
      {
      }
    }
  }
}
