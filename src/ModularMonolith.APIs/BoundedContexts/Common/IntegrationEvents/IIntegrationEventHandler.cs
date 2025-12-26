namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

public interface IIntegrationEventHandler
{ }

public interface IIntegrationEventHandler<in TIntegrationEvent>
where TIntegrationEvent
: IIntegrationEvent
{
  ValueTask HandleAsync(
    TIntegrationEvent @event
  , CancellationToken cancellationToken);
}
