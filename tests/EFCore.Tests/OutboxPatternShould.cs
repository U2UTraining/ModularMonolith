using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
  public async Task PublishMessagesInsertedInOutboxTable()
  {
    // TCS is signalled by the mock publisher as soon as the event is delivered,
    // replacing the arbitrary Task.Delay(1000) with a deterministic wait.
    TaskCompletionSource published = new(TaskCreationOptions.RunContinuationsAsynchronously);

    IIntegrationEventPublisher pub = Substitute.For<IIntegrationEventPublisher>();
    pub.When(p => p.PublishIntegrationEventAsync(Arg.Any<IIntegrationEvent>(), Arg.Any<CancellationToken>()))
       .Do(_ => published.TrySetResult());

    IServiceCollection services = new ServiceCollection();
    services.AddDbContext<CurrenciesDb>(options =>
    {
      options.UseSqlServer(_fixture.ConnectionString);
    });
    services.AddSingleton<IIntegrationEventPublisher>(pub);
    services.AddSingleton<ILogger<OutboxHostedService<CurrenciesDb>>>(
      NullLogger<OutboxHostedService<CurrenciesDb>>.Instance
    );
    services
      .AddKeyedSingleton<IOutboxSignal, OutboxSignal>(nameof(CurrenciesDb))
      .AddHostedService<OutboxHostedService<CurrenciesDb>>();

    IServiceProvider serviceProvider = services.BuildServiceProvider();

    using IServiceScope scope = serviceProvider.CreateScope();

    OutboxHostedService<CurrenciesDb> hostedService =
      serviceProvider.GetServices<IHostedService>()
        .OfType<OutboxHostedService<CurrenciesDb>>()
        .First();

    Task runner = Task.Run(() => hostedService.StartAsync(TestContext.Current.CancellationToken), TestContext.Current.CancellationToken);

    IOutboxSignal signal = serviceProvider
      .GetRequiredKeyedService<IOutboxSignal>(nameof(CurrenciesDb));
    using CurrenciesDb db = serviceProvider.GetRequiredService<CurrenciesDb>();

    TestIntegrationEvent @event = new()
    {
      EventId = Guid.NewGuid()
    };
    await db.SaveChangesAsync(@event, signal, CancellationToken.None);

    // Block until the background service publishes the event — no arbitrary timeout.
    await published.Task.WaitAsync(TestContext.Current.CancellationToken);

    await pub.Received(requiredNumberOfCalls: 1)
      .PublishIntegrationEventAsync(
        Arg.Is<TestIntegrationEvent>(ev => ev.EventId == @event.EventId)
      , Arg.Any<CancellationToken>()
      );
  }
}
