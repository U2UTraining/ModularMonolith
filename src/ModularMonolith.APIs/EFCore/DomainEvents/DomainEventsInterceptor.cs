namespace ModularMonolith.APIs.EFCore.DomainEvents;

//class DomainEventsInterceptor
//{
//  //https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/diagnostic-listeners
//}

public class DomainEventsInterceptor(IDomainEventPublisher domainEventPublisher)
  : SaveChangesInterceptor
{
  public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
      DbContextEventData eventData,
      InterceptionResult<int> result,
      CancellationToken cancellationToken = default)
  {
    if (eventData.Context is not null)
    {
      foreach (EntityBase entity in EntityBaseEntries(eventData.Context))
      {
        await entity.DispatchDomainEvents(domainEventPublisher, cancellationToken);
      }
    }
    return await base.SavingChangesAsync(eventData, result, cancellationToken);

    IEnumerable<EntityBase> EntityBaseEntries(DbContext context)
    {
      return context
        .ChangeTracker
        .Entries<EntityBase>()
        .Select(entry => entry.Entity)
        //.Where(e => e.State is EntityState.Deleted)
        ;
    }
  }
}
