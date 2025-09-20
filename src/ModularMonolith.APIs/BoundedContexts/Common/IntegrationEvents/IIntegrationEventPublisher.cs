namespace ModularMonolith.BoundedContexts.Common.IntegrationEvents;

public interface IIntegrationEventPublisher
{
  ValueTask PublishIntegrationEventAsync(
    IIntegrationEvent @event
  , CancellationToken cancellationToken = default
  );
}
