using TUnit.Core;

namespace ModularMonolith.APIs.Tests.EFCore.OutboxPattern;

[TUnit.Core.Category("IntegrationTests")]

/// <summary>
/// Verifies the outbox pattern publishes integration events that are
/// inserted during SaveChanges. Uses a per-test SQL Server container
/// following the same lifecycle pattern as the other test classes.
/// </summary>
public class OutboxPatternShould : IAsyncDisposable
{
  private readonly MsSqlContainer _sqlContainer =
    new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
    .Build();

  [Before(Test)]
  public async Task Setup()
  {
    await _sqlContainer.StartAsync();

    DbContextOptions<CurrenciesDb> options =
      new DbContextOptionsBuilder<CurrenciesDb>()
        .UseSqlServer(_sqlContainer.GetConnectionString())
        .ConfigureWarnings(w
          => w.Ignore(RelationalEventId.PendingModelChangesWarning))
        .Options;

    using CurrenciesDb db = new(options);
    await db.Database.EnsureCreatedAsync();
  }

  [After(Test)]
  public async Task Teardown()
  {
    await _sqlContainer.StopAsync();
  }

  public async ValueTask DisposeAsync()
  {
    await _sqlContainer.DisposeAsync();
  }

  [Test]
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
      options.UseSqlServer(_sqlContainer.GetConnectionString());
    });
    services.AddSingleton<IIntegrationEventPublisher>(pub);
    services.AddSingleton<ILogger<OutboxHostedService<CurrenciesDb>>>(
      NullLogger<OutboxHostedService<CurrenciesDb>>.Instance
    );
    services
      .AddKeyedSingleton<IOutboxSignal, OutboxSignal>(nameof(CurrenciesDb))
      .AddHostedService<OutboxHostedService<CurrenciesDb>>();

    ServiceProvider serviceProvider = services.BuildServiceProvider();
    await using (serviceProvider.ConfigureAwait(false))
    {
      OutboxHostedService<CurrenciesDb> hostedService =
        serviceProvider.GetServices<IHostedService>()
          .OfType<OutboxHostedService<CurrenciesDb>>()
          .First();

      using CancellationTokenSource cts = new(TimeSpan.FromSeconds(30));

      Task runner = Task.Run(
        () => hostedService.StartAsync(cts.Token), cts.Token);

      IOutboxSignal signal = serviceProvider
        .GetRequiredKeyedService<IOutboxSignal>(nameof(CurrenciesDb));
      using CurrenciesDb db = serviceProvider.GetRequiredService<CurrenciesDb>();

      TestIntegrationEvent @event = new()
      {
        EventId = Guid.NewGuid()
      };
      await db.SaveChangesAsync(@event, signal, CancellationToken.None);

      // Block until the background service publishes the event.
      await published.Task.WaitAsync(cts.Token);

      await pub.Received(requiredNumberOfCalls: 1)
        .PublishIntegrationEventAsync(
          Arg.Is<TestIntegrationEvent>(ev => ev.EventId == @event.EventId)
        , Arg.Any<CancellationToken>()
        );
    }
  }
}
