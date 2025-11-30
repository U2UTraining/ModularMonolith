namespace ModularMonolith.BoundedContexts.Common.IntegrationEvents;

public interface IIntegrationEventHandler
{ }

public interface IIntegrationEventHandler<TIntegrationEvent>
where TIntegrationEvent
: IIntegrationEvent
{
  ValueTask HandleAsync(
    TIntegrationEvent @event
  , CancellationToken cancellationToken);
}
