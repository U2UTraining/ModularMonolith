using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;

using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;
using ModularMonolith.APIs.BoundedContexts.Currencies.Infra;
using ModularMonolith.APIs.EFCore.OutboxPattern;

using NSubstitute;

using Testcontainers.MsSql;

namespace EFCore.Tests;

[Collection("SqlServer")]
public class OutboxPatternShould
{
  public OutboxPatternShould(SqlServerContainerFixture fixture)
  {
    _fixture = fixture;
  }

  private readonly SqlServerContainerFixture _fixture;

  [Fact]
  public async Task Test1()
  {
    CancellationTokenSource cts = new();

      IIntegrationEventPublisher pub = Substitute.For<IIntegrationEventPublisher>();

    IServiceCollection services = new ServiceCollection();
    services.AddDbContext<CurrenciesDb>(options =>
    {
      options.UseSqlServer(_fixture.ConnectionString);
    });
    services.AddSingleton<IIntegrationEventPublisher>(pub);
    //services.AddSingleton<ILogger>(NullLogger.Instance);
    services.AddSingleton<ILogger<OutboxHostedService<CurrenciesDb>>>(
      NullLogger<OutboxHostedService<CurrenciesDb>>.Instance
    );

    services
      .AddKeyedSingleton<IOutboxSignal, OutboxSignal>(nameof(CurrenciesDb))
      .AddHostedService<OutboxHostedService<CurrenciesDb>>();

    IServiceProvider serviceProvider = services.BuildServiceProvider();

    using (var scope = serviceProvider.CreateScope())
    {

      OutboxHostedService<CurrenciesDb> hostedService =
        //serviceProvider.GetRequiredService<OutboxHostedService<CurrenciesDb>>();

      serviceProvider.GetServices<IHostedService>()
        .OfType<OutboxHostedService<CurrenciesDb>>()
        .First();

      Task runner = Task.Run(() => hostedService.StartAsync(TestContext.Current.CancellationToken), TestContext.Current.CancellationToken);

      IOutboxSignal signal = scope.ServiceProvider
        .GetRequiredKeyedService<IOutboxSignal>(nameof(CurrenciesDb));
      using CurrenciesDb db = serviceProvider.GetRequiredService<CurrenciesDb>();

      TestIntegrationEvent @event = new()
      {
        EventId = Guid.NewGuid()
      };
      await db.SaveChangesAsync(@event, signal, CancellationToken.None);

      await Task.Delay(1000, TestContext.Current.CancellationToken);

      await pub.Received(requiredNumberOfCalls: 1)
        .PublishIntegrationEventAsync(
          Arg.Is<TestIntegrationEvent>(ev => ev.EventId == @event.EventId)
        , Arg.Any<CancellationToken>()
      );
    }
  }
}
