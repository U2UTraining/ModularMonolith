namespace ModularMonolith.APIs.EFCore.OutboxPattern;

/// <summary>
/// Allows the outbox writer to wake the <see cref="OutboxHostedService{DB}"/>
/// immediately when a new message is enqueued, instead of waiting for the next
/// poll interval.
/// </summary>
public interface IOutboxSignal
{
  /// <summary>
  /// Signals that at least one new outbox message is available for processing.
  /// Safe to call from any thread. Redundant signals (before the previous one
  /// is consumed) are silently ignored.
  /// </summary>
  void Signal();

  /// <summary>
  /// Asynchronously waits until a signal is received or <paramref name="timeout"/> elapses.
  /// </summary>
  Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken);
}
