namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

/// <summary>
/// Publishes integration events to a channel for processing.
/// </summary>
/// <remarks>
/// There is a hosted service that reads from the channel
/// and processes the integration events.
/// </remarks>
public class U2UIntegrationEventPublisher
: IIntegrationEventPublisher
{
  private readonly Channel<IIntegrationEvent> _channel;

  public U2UIntegrationEventPublisher(
    Channel<IIntegrationEvent> channel
  ) => _channel = channel;

  public async ValueTask PublishIntegrationEventAsync(
    IIntegrationEvent @event
  , CancellationToken cancellationToken = default
  ) => await _channel.Writer.WriteAsync(@event, cancellationToken);
}
