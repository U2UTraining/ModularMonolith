namespace ModularMonolithEFCore.Auditability;

/// <summary>
/// EF Core Interceptor taking care of using maintenance columns
/// Entities will see their created column set when inserted, and update column when inserted/updated
/// </summary>
public sealed class AuditabilityInterceptor 
: SaveChangesInterceptor
{
  private readonly string _utcCreated;
  private readonly string _utcModified;

  public AuditabilityInterceptor(
    string utcCreated = Auditability.UtcCreated
  , string utcModified = Auditability.UtcModified)
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

      foreach (EntityEntry<IAuditability> upsertable 
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

    IEnumerable<EntityEntry<IAuditability>> EntriesToUpsert(DbContext context)
    {
      return context
        .ChangeTracker
        .Entries<IAuditability>()
        .Where(e => e.State is EntityState.Added or EntityState.Modified);
    }
  }
}
