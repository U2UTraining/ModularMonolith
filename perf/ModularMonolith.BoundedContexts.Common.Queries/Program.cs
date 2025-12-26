using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace ModularMonolith.BoundedContexts.Common.Queries;

public static class Program
{
  public static void Main(string[] args)
  {
    IConfig config = DefaultConfig.Instance;

    _ = BenchmarkRunner.Run<Benchmarks>(config, args);

    // Use this to select benchmarks from the console:
    // var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
  }
}