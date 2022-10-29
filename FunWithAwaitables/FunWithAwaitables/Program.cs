
//await 1000;

using FunWithAwaitables;
using System.Diagnostics;

var stopwatch = new Stopwatch();
stopwatch.Start();

await MagicDelay.Milliseconds(1000);

Console.WriteLine(stopwatch.ElapsedMilliseconds);

await TimeSpan.FromMilliseconds(2000);

Console.WriteLine(stopwatch.ElapsedMilliseconds);

await 1000;

Console.WriteLine(stopwatch.ElapsedMilliseconds);
