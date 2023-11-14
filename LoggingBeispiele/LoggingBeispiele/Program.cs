
using BenchmarkDotNet.Running;
using LoggingBeispiele;

var summary = BenchmarkRunner.Run<BenchmarkTests>();
Console.WriteLine(summary);