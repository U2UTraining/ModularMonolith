namespace ModularMonolithBoundedContexts.Common.DomainEvents;

public interface IDomainEventHandler
{ }

/// <summary>
/// An IDomainEventHandler handles a domain event, within the 
/// context of a bounded context.
/// </summary>
/// <typeparam name="TDomainEvent">
/// Type of domain event
/// </typeparam>
/// <remarks>
/// A domain event handler should be registered in DI.
/// There can be multiple handlers for the same domain event.
/// </remarks>
public interface IDomainEventHandler<TDomainEvent>
: IDomainEventHandler
where TDomainEvent
: IDomainEvent
{
  ValueTask HandleAsync(
    TDomainEvent @event
  , CancellationToken cancellationToken);
}
