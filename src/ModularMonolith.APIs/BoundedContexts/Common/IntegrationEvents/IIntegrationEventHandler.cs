namespace ModularMonolithBoundedContexts.Common.IntegrationEvents;

public interface IIntegrationEventHandler
{ }

public interface IIntegrationEventHandler<TIntegrationEvent>
: IIntegrationEventHandler
where TIntegrationEvent
: IIntegrationEvent
{
  ValueTask HandleAsync(
    TIntegrationEvent @event
  , CancellationToken cancellationToken);
}
