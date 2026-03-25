namespace ModularMonolith.APIs.EFCore.OutboxPattern;

/// <summary>
/// Singleton signal that wakes the outbox hosted service when new messages are
/// written. Uses a <see cref="SemaphoreSlim"/> capped at 1 so that multiple rapid
/// writes coalesce into a single wake-up.
/// </summary>
public sealed class OutboxSignal : IOutboxSignal, IDisposable
{
  // Max count of 1 ensures multiple calls to Signal() before the service wakes
  // are treated as a single notification.
  private readonly SemaphoreSlim _semaphore = new(initialCount: 0, maxCount: 1);

  /// <inheritdoc/>
  public void Signal()
  {
    try
    {
      _semaphore.Release();
    }
    catch (SemaphoreFullException)
    {
      // A signal is already pending and has not been consumed yet — safe to ignore.
    }
  }

  /// <inheritdoc/>
  public async Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
    => await _semaphore.WaitAsync(timeout, cancellationToken);

  /// <inheritdoc/>
  public void Dispose() => _semaphore.Dispose();
}
