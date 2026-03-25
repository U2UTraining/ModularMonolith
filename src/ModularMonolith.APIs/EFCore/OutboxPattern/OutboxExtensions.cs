namespace ModularMonolith.APIs.EFCore.OutboxPattern;

/// <summary>
/// Static helpers for writing integration events into the outbox.
/// <para>
/// Callers add an event via <see cref="SendIntegrationEvent"/> before calling
/// <c>SaveChangesAsync</c>. Because both the domain change and the outbox row
/// are written in the same EF Core unit-of-work, they share a single database
/// transaction — eliminating the dual-write race condition where the app could
/// crash after saving the domain change but before publishing the event.
/// </para>
/// </summary>
public static class OutboxExtensions
{
  /// <summary>
  /// Stages an <see cref="OutboxMessage"/> for the given event in the
  /// <see cref="DbContext"/> change tracker and immediately signals the outbox
  /// hosted service so it wakes without waiting for its next poll interval.
  /// The row is only written to the database when the caller subsequently
  /// calls <c>SaveChangesAsync</c>.
  /// </summary>
  public static async Task SendIntegrationEvent(
    this DbContext db
  , IIntegrationEvent @event
  , IOutboxSignal outboxSignal
  , CancellationToken cancellationToken)
  {
    OutboxMessage msg = new OutboxMessage(@event);
    db.Set<OutboxMessage>().Add(msg);
    await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // Signal after SaveChangesAsync: the hosted service will wake and query
    // the database. If the transaction hasn't committed yet it finds nothing
    // and goes back to sleep — a harmless extra round-trip that avoids
    // introducing latency on the happy path.
    outboxSignal.Signal();
  }
}
