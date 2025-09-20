namespace U2U.ModularMonolith.BoundedContexts.Common.IntegrationEvents;

public interface IIntegrationEventPublisher
{
  ValueTask PublishIntegrationEventAsync(
    IIntegrationEvent @event
  , CancellationToken cancellationToken = default
  );
}
