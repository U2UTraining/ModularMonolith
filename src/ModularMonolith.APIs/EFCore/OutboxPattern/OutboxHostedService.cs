namespace ModularMonolith.APIs.EFCore.OutboxPattern;

/// <summary>
/// Background service that processes pending outbox messages and publishes them
/// as integration events.
/// <para>
/// Rather than busy-polling the database on a fixed timer, the service blocks on
/// an <see cref="IOutboxSignal"/> that is fired by
/// <see cref="OutboxExtensions.SaveChangesAsync"/> the moment a message is
/// staged. This keeps end-to-end latency close to zero under normal load while
/// completely eliminating unnecessary database queries when the table is empty.
/// A fallback timeout (<see cref="FallbackPollInterval"/>) ensures the service
/// still recovers any messages that were committed before the service started or
/// whose signal was missed.
/// </para>
/// </summary>
public class OutboxHostedService<DB>
  : BackgroundService
where
  DB : DbContext
{
  /// <summary>
  /// Maximum rows fetched per database round-trip. Keeping this bounded prevents
  /// a large backlog from exhausting memory in a single allocation.
  /// </summary>
  private const int BatchSize = 50;

  /// <summary>
  /// How long to wait for a signal before doing a precautionary database poll.
  /// This guards against messages that were committed to the database before the
  /// service started, or in the unlikely event that a signal was dropped.
  /// </summary>
  private static readonly TimeSpan FallbackPollInterval = TimeSpan.FromSeconds(1);

  private readonly IServiceScopeFactory _scopeFactory;
  private readonly IIntegrationEventPublisher _integrationEventPublisher;
  private readonly IOutboxSignal _outboxSignal;
  private readonly ILogger<OutboxHostedService<DB>> _logger;

  public OutboxHostedService(
    IServiceScopeFactory scopeFactory
  , IIntegrationEventPublisher integrationEventPublisher
  , [FromKeyedServices(nameof(CurrenciesDb))] IOutboxSignal outboxSignal
  , ILogger<OutboxHostedService<DB>> logger)
  {
    _scopeFactory = scopeFactory;
    _integrationEventPublisher = integrationEventPublisher;
    _outboxSignal = outboxSignal;
    _logger = logger;
  }

  protected override async Task ExecuteAsync(
    CancellationToken cancellationToken)
  {
    while (!cancellationToken.IsCancellationRequested)
    {
      // Sleep until a message arrives or the fallback interval elapses.
      // This replaces the original Task.Delay(5s) polling loop, which ran a
      // database query every five seconds even when the outbox was empty.
      await _outboxSignal.WaitAsync(FallbackPollInterval, cancellationToken);

      try
      {
        // Drain all available batches in one wake-up cycle rather than waking once
        // per batch. This handles bursts (e.g. bulk imports) without accumulating
        // a queue of pending signals.
        bool hadMore = true;
        while (hadMore && !cancellationToken.IsCancellationRequested)
        {
          hadMore = await ProcessBatchAsync(cancellationToken);
        }
      }
      catch (Exception ex) when (ex is not OperationCanceledException)
      {
        // Catching here prevents a transient infrastructure error (e.g. a momentary
        // database outage) from terminating the hosted service permanently. The next
        // signal or fallback poll will retry.
        _logger.LogError(ex, "Unhandled error while processing outbox messages.");
      }
    }
  }

  /// <summary>
  /// Fetches up to <see cref="BatchSize"/> pending messages, publishes each one,
  /// and marks them as processed in a single <c>SaveChangesAsync</c> call.
  /// </summary>
  /// <returns>
  /// <c>true</c> when a full batch was read, signalling that more rows may still
  /// be waiting; <c>false</c> when fewer than <see cref="BatchSize"/> rows were
  /// returned, meaning the table is now empty (or nearly so).
  /// </returns>
  private async Task<bool> ProcessBatchAsync(CancellationToken cancellationToken)
  {
    // A new scope is created per batch because DbContext is a scoped service and
    // must not be shared across async continuations that span multiple batches.
    using IServiceScope scope = _scopeFactory.CreateScope();
    DB db = scope.ServiceProvider.GetRequiredService<DB>();

    // .Take(BatchSize) ensures we never load an unbounded number of rows into
    // memory in one shot, regardless of how large the backlog grows.
    // The global query filter on OutboxMessage automatically restricts results
    // to rows where UtcProcessed IS NULL, so no explicit .Where() is needed.
    List<OutboxMessage> messages = await db.Set<OutboxMessage>()
      .Take(BatchSize)
      .ToListAsync(cancellationToken);

    if (messages.Count == 0)
    {
      return false;
    }

    foreach (OutboxMessage msg in messages)
    {
      IIntegrationEvent? @event = msg.DeserializePayload();
      if (@event is null)
      {
        _logger.LogWarning(
          "Outbox message {Id} has unresolvable type '{EventType}' and will be skipped.",
          msg.Id, msg.EventType);

        // Mark unresolvable messages as processed so they are never retried.
        // Without this, a single bad row would block all subsequent messages forever.
        msg.UtcProcessed = DateTime.UtcNow;
        continue;
      }

      await _integrationEventPublisher.PublishIntegrationEventAsync(@event, cancellationToken);

      // UtcProcessed is set after a successful publish so that any exception thrown
      // by the publisher leaves the row unmarked. On the next iteration the message
      // will be retried automatically (at-least-once delivery semantics).
      msg.UtcProcessed = DateTime.UtcNow;
    }

    // A single SaveChangesAsync at the end of the batch is more efficient than
    // one per message, and all processed timestamps are committed atomically.
    await db.SaveChangesAsync(cancellationToken);

    // If exactly BatchSize rows were returned there are likely more rows waiting —
    // tell the caller to loop immediately rather than going back to sleep.
    return messages.Count == BatchSize;
  }
}
