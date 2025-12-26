using System;
using System.Threading.Tasks;

using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.DI;
using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.BoundedContexts.Common.Queries;

[MemoryDiagnoser]
public class Benchmarks
{
  private IQuerySender querySender = default!;
  private ICommandSender commandSender = default!;

  [GlobalSetup]
  public async Task Setup()
  {
    IServiceCollection services = new ServiceCollection();
    services.AddScoped<IQuerySender, U2UQuerySender>();
    services.AddScoped<IQueryHandler<SimpleQuery, int>, SimpleQueryHandler>();
    services.AddScoped<ICommandSender, U2UCommandSender>();
    services.AddScoped<ICommandHandler<SimpleCommand, int>, SimpleCommandHandler>();
    IServiceProvider serviceProvider = services.BuildServiceProvider();
    querySender = serviceProvider.GetRequiredService<IQuerySender>();
    _ = await querySender.AskAsync(new SimpleQuery(), CancellationToken.None);
    commandSender = serviceProvider.GetRequiredService<ICommandSender>();
    _ = await commandSender.ExecuteAsync(new SimpleCommand(), CancellationToken.None);
  }

  [Benchmark]
  public async Task ExecutingASimpleQuery()
  {
    _ = await querySender.AskAsync(new SimpleQuery(), CancellationToken.None);
  }

  [Benchmark]
  public async Task ExecutingASimpleCommand()
  {
    _ = await commandSender.ExecuteAsync(new SimpleCommand(), CancellationToken.None);
  }
}
