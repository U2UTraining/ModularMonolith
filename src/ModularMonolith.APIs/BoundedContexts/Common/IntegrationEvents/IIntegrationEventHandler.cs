namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

public interface IIntegrationEventHandler
{ }

public interface IIntegrationEventHandler<in TIntegrationEvent>
  : IIntegrationEventHandler
where TIntegrationEvent
: IIntegrationEvent
{
  ValueTask HandleAsync(
    TIntegrationEvent @event
  , CancellationToken cancellationToken);
}
