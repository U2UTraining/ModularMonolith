namespace U2U.ModularMonolith.BoundedContexts.Common.Repositories;

/// <summary>
/// This is an automatic implementation for IRepository.
/// </summary>
/// <typeparam name="T">The entity class' type.</typeparam>
/// <typeparam name="D">The DbContext to use.</typeparam>
public class Repository<T, D>
: ReadonlyRepository<T, D>
, IRepository<T>
where T 
: class
, IAggregateRoot
where D 
: DbContext
{
  private readonly IDomainEventPublisher _domainEventPublisher;

  public Repository(
    D dbContext
  , IDomainEventPublisher domainEventPublisher) 
  : base(dbContext)
  {
    _domainEventPublisher = domainEventPublisher;
  }

  public virtual ValueTask InsertAsync(
    T entity
  , CancellationToken token)
  {
    _ = DbContext.Set<T>().Add(entity);
    return ValueTask.CompletedTask;
  }

  public virtual ValueTask DeleteAsync(
    T entity
  , CancellationToken token)
  {
    _ = DbContext.Set<T>().Remove(entity);
    return ValueTask.CompletedTask;
  }

  public virtual ValueTask UpdateAsync(
    T entity
  , CancellationToken token)
  {
    DbContext.Entry(entity).State = EntityState.Modified;
    return ValueTask.CompletedTask;
  }

  // Domain events are triggered as part of the same transaction
  private async Task DispatchEvents(
    DbContext db
  , CancellationToken cancellationToken)
  {
    foreach (EntityBase entity in EntityBaseEntriesWithEvents(db))
    {
      await entity.DispatchDomainEvents(_domainEventPublisher, cancellationToken);
    }

    // Filter method returning all entities that have domain events
    IEnumerable<EntityBase> EntityBaseEntriesWithEvents(DbContext context)
    {
      return context
        .ChangeTracker
        .Entries<EntityBase>()
        .Select(ee => ee.Entity)
        .Where(e => e.HasEvents)
        ;
    }
  }

  /// <summary>
  /// Save all changes
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <remarks>Triggers all domain events as part of the same transaction</remarks>
  public virtual async ValueTask SaveChangesAsync(
    CancellationToken cancellationToken)
  {
    await DispatchEvents(DbContext, cancellationToken);
    await DbContext.SaveChangesAsync(cancellationToken);
  }
}

