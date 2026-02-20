namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

/// <summary>
/// Allows multiple subscribers to receive the same items from a single source channel.
/// </summary>
/// <typeparam name="T">The type of items in the channel.</typeparam>
public class ChannelMultiplexer<T> : IAsyncDisposable
{
  private readonly Channel<T> _inner;
  // Using SemaphoreSlim for async locking
  private readonly SemaphoreSlim _semaphore = new(1, 1);
  private readonly List<Channel<T>> _subscribers = [];
  private bool _isDisposed;

  public ChannelMultiplexer(Channel<T> inner)
    => this._inner = inner ?? throw new ArgumentNullException(nameof(inner));

  public async Task<Channel<T>> SubscribeAsync(CancellationToken cancellationToken = default)
  {
    ObjectDisposedException.ThrowIf(this._isDisposed, this);

    var channel = Channel.CreateUnbounded<T>();
    await this._semaphore.WaitAsync(cancellationToken);
    try
    {
      this._subscribers.Add(channel);
    }
    finally
    {
      _ = this._semaphore.Release();
    }

    return channel;
  }

  public async Task UnsubscribeAsync(Channel<T> channel, CancellationToken cancellationToken = default)
  {
    if (channel == null)
    {
      return;
    }

    await this._semaphore.WaitAsync(cancellationToken);
    try
    {
      _ = this._subscribers.Remove(channel);
    }
    finally
    {
      _ = this._semaphore.Release();
    }

    channel.Writer.Complete();
  }

  public async Task PublishAsync(T item, CancellationToken cancellationToken = default)
  {
    ObjectDisposedException.ThrowIf(this._isDisposed, this);

    Channel<T>[] subscribersCopy;
    await this._semaphore.WaitAsync(cancellationToken);
    try
    {
      subscribersCopy = this._subscribers.ToArray();
    }
    finally
    {
      _ = this._semaphore.Release();
    }

    List<Channel<T>>? toRemove = null;

    foreach (Channel<T> subscriber in subscribersCopy)
    {
      try
      {
        if (!subscriber.Writer.TryWrite(item))
        {
          await subscriber.Writer.WriteAsync(item, cancellationToken);
        }
      }
      catch (ChannelClosedException)
      {
        toRemove ??= [];
        toRemove.Add(subscriber);
      }
    }

    // Batch remove closed subscribers
    if (toRemove != null)
    {
      await this._semaphore.WaitAsync(cancellationToken);
      try
      {
        foreach (Channel<T> subscriber in toRemove)
        {
          _ = this._subscribers.Remove(subscriber);
        }
      }
      finally
      {
        _ = this._semaphore.Release();
      }
    }
  }

  public async Task MultiplexAsync(CancellationToken cancellationToken = default)
  {
    ObjectDisposedException.ThrowIf(this._isDisposed, this);

    try
    {
      await foreach (T? item in this._inner.Reader.ReadAllAsync(cancellationToken))
      {
        await PublishAsync(item, cancellationToken);
      }
    }
    finally
    {
      await CompleteAllSubscribersAsync();
    }
  }

  private async Task CompleteAllSubscribersAsync()
  {
    Channel<T>[] subscribersCopy;
    await this._semaphore.WaitAsync();
    try
    {
      subscribersCopy = this._subscribers.ToArray();
      this._subscribers.Clear();
    }
    finally
    {
      _ = this._semaphore.Release();
    }

    foreach (Channel<T> subscriber in subscribersCopy)
    {
      subscriber.Writer.Complete();
    }
  }

  public async ValueTask DisposeAsync()
  {
    if (this._isDisposed)
    {
      return;
    }

    this._isDisposed = true;

    await CompleteAllSubscribersAsync();
    this._semaphore.Dispose();

    GC.SuppressFinalize(this);
  }
}
