using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingBeispiele
{
  public record struct Person(int Id, string Name);

  [MemoryDiagnoser]
  public partial class BenchmarkTests
  {
    private const string Message = "Testing a message by {Person} and a {Time}";
    private readonly ILogger _logger;
    private readonly Person _person;
    private readonly DateTime _startTime;

    [LoggerMessage(Level = LogLevel.Information, Message = Message)]
    partial void LogEnabled(Person person, DateTime time);

    [LoggerMessage(Level = LogLevel.Debug, Message = Message)]
    partial void LogDisabled(Person person, DateTime time);

    [LoggerMessage(Message = Message)]
    partial void LogDynamicLevel(LogLevel level, Person person, DateTime time);

    public BenchmarkTests()
    {
      var loggerFactory = new LoggerFactory([new InMemoryProvider()]);
      _logger = loggerFactory.CreateLogger(nameof(BenchmarkTests));
      _person = new Person(123, "Joe Blogs");
      _startTime = DateTime.UtcNow;
    }

    //[Benchmark]
    //public void InterpolatedEnabled()
    //{
    //  _logger.LogInformation($"Testing a message by {_person} and a {_startTime}");
    //}

    //[Benchmark]
    //public void InterpolatedDisabled()
    //{
    //  _logger.LogDebug($"Testing a message by {_person} and a {_startTime}");
    //}

    //[Benchmark]
    //public void DirectCallEnabled()
    //{
    //  _logger.LogInformation(Message, _person, _startTime);
    //}

    //[Benchmark]
    //public void DirectCallDisabled()
    //{
    //  _logger.LogDebug(Message, _person, _startTime);
    //}

    //[Benchmark]
    //public void IsEnabledCheckEnabled()
    //{
    //  if (_logger.IsEnabled(LogLevel.Information))
    //  {
    //    _logger.LogInformation(Message, _person, _startTime);
    //  }
    //}

    //[Benchmark]
    //public void IsEnabledCheckDisabled()
    //{
    //  if (_logger.IsEnabled(LogLevel.Debug))
    //  {
    //    _logger.LogDebug(Message, _person, _startTime);
    //  }
    //}

    [Benchmark]
    public void SourceGeneratorEnabled()
    {
      LogEnabled(_person, _startTime);
    }

    [Benchmark]
    public void SourceGeneratorDisabled()
    {
      LogDisabled(_person, _startTime);
    }

    [Benchmark]
    public void SourceGeneratorDynamicLevelEnabled()
    {
      LogDynamicLevel(LogLevel.Information, _person, _startTime);
    }

    [Benchmark]
    public void SourceGeneratorDynamicLevelDisabled()
    {
      LogDynamicLevel(LogLevel.Debug, _person, _startTime);
    }

    [Benchmark]
    public void LogDirect()
    {
      var info = new LogInformation() { Person = _person, StartTime = _startTime };
      _logger.Log<LogInformation>(LogLevel.Warning, new(123), info, null, (li, _) => $"Testing a message by {li.Person} and a {li.StartTime}");
    }

    struct LogInformation
    {
      public Person Person { get; set; }
      public DateTime StartTime { get; set; }
    }

  }
}
