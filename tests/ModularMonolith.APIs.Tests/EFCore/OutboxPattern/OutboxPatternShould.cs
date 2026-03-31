using TUnit.Core;

namespace ModularMonolith.APIs.Tests.EFCore.OutboxPattern;

/// <summary>
/// Uses a globally-shared SqlServerContainerFixture so the container is started once
/// for all tests in this class, matching the previous xUnit [Collection] behaviour.
/// </summary>
[ClassDataSource<SqlServerContainerFixture>(Shared = SharedType.PerTestSession)]
public class OutboxPatternShould(SqlServerContainerFixture fixture)
{
  [Test]
  public async Task PublishMessagesInsertedInOutboxTable(CancellationToken cancellationToken)
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
      options.UseSqlServer(fixture.ConnectionString);
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

    Task runner = Task.Run(() => hostedService.StartAsync(cancellationToken), cancellationToken);

    IOutboxSignal signal = serviceProvider
      .GetRequiredKeyedService<IOutboxSignal>(nameof(CurrenciesDb));
    using CurrenciesDb db = serviceProvider.GetRequiredService<CurrenciesDb>();

    TestIntegrationEvent @event = new()
    {
      EventId = Guid.NewGuid()
    };
    await db.SaveChangesAsync(@event, signal, CancellationToken.None);

    // Block until the background service publishes the event — no arbitrary timeout.
    await published.Task.WaitAsync(cancellationToken);

    await pub.Received(requiredNumberOfCalls: 1)
      .PublishIntegrationEventAsync(
        Arg.Is<TestIntegrationEvent>(ev => ev.EventId == @event.EventId)
      , Arg.Any<CancellationToken>()
      );
  }
}
