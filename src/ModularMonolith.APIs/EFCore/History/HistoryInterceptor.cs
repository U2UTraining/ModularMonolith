namespace ModularMonolithEFCore.History;

/// <summary>
/// EF Core Interceptor taking care of using maintenance columns
/// Entities will see their created column set when inserted, and update column when inserted/updated
/// </summary>
public sealed class HistoryInterceptor 
: SaveChangesInterceptor
{
  private readonly string _utcCreated;
  private readonly string _utcModified;

  public HistoryInterceptor(
    string utcCreated = History.UtcCreated
  , string utcModified = History.UtcModified)
  {
    _utcCreated = utcCreated;
    _utcModified = utcModified;
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData
  , InterceptionResult<int> result
  , CancellationToken cancellationToken = default)
  {
    if (eventData.Context is not null)
    {
      DateTime utcNow = DateTime.UtcNow;

      foreach (EntityEntry<IHistory> upsertable 
      in EntriesToUpsert(eventData.Context))
      {
        if (upsertable.State is EntityState.Added)
        {
          upsertable.Property(_utcCreated).CurrentValue = utcNow;
        }
        upsertable.Property(_utcModified).CurrentValue = utcNow;
      }
    }
    return base.SavingChangesAsync(eventData, result, cancellationToken);

    IEnumerable<EntityEntry<IHistory>> EntriesToUpsert(DbContext context)
    {
      return context
        .ChangeTracker
        .Entries<IHistory>()
        .Where(e => e.State is EntityState.Added or EntityState.Modified);
    }
  }
}
