namespace ModularMonolith.BoundedContexts.Common.DomainEvents;

/// <summary>
/// Publish a Domain Event to all IDomainEventHandlers
/// </summary>
/// <remarks>
/// Pushing is synchronous, part of SaveChangesAsync. 
/// Scoped instances are shared.
/// </remarks>
public interface IDomainEventPublisher
{
  ValueTask PublishAsync(
    IDomainEvent @event
  , CancellationToken cancellationToken = default
  );
}
