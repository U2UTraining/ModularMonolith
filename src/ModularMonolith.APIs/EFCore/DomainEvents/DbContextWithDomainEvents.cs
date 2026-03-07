namespace ModularMonolith.APIs.EFCore.DomainEvents;

public class DbContextWithDomainEvents
  : DbContext
{
  private ICollection<IDomainEvent>? _domainEvents;

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

}
