namespace ModularMonolith.APIs.BoundedContexts.Common.Entities;

/// <summary>
/// Base class for Entities, supporting Domain Events
/// </summary>
public abstract class EntityBase
{
  public ICollection<IDomainEvent>? _domainEvents;

  public void RegisterDomainEvent(IDomainEvent @event)
  {
    _domainEvents ??= [];
    _domainEvents.Add(@event);
  }

  public void UnregisterDomainEvent(IDomainEvent @event)
  => _ = _domainEvents?.Remove(@event);

  public void ClearDomainEvents()
  => _domainEvents?.Clear();

  public bool HasEvents
  => _domainEvents?.Any() ?? false;

  public async Task DispatchDomainEvents(IDomainEventPublisher mediator
    , CancellationToken cancellationToken)
  {
    if (_domainEvents is not null)
    {
      foreach (IDomainEvent @event in _domainEvents)
      {
        await mediator.PublishAsync(@event, cancellationToken);
      }
      ClearDomainEvents();
    }
  }
}

/// <summary>
/// EntityBase serves as the base class for all entities.
/// However, this is not required for all entities.
/// </summary>
/// <remarks>
/// Entities have an identity, here represented by PK
/// </remarks>
public abstract class EntityBase<PK>
: EntityBase
{
  public EntityBase(PK id)
  => Id = id;

  public PK Id { get; }
}

