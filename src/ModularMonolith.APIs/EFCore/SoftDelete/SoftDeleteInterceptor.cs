namespace ModularMonolithEFCore.SoftDelete;

/// <summary>
/// EF Core Interceptor taking care of using soft delete
/// Entities are not deleted, instead marked as deleted using a bit column.
/// </summary>
public sealed class SoftDeleteInterceptor 
: SaveChangesInterceptor
{
  private readonly string _isDeleted;
  private readonly string _utcDeleted;

  public SoftDeleteInterceptor(
    string isDeleted = SoftDeleteable.IsDeleted
  , string utcDeleted = SoftDeleteable.UtcDeleted)
  {
    _isDeleted = isDeleted;
    _utcDeleted = utcDeleted;
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData
  , InterceptionResult<int> result
  , CancellationToken cancellationToken = default)
  {
    if (eventData.Context is not null)
    {
      DateTime utcNow = DateTime.UtcNow;
      foreach (EntityEntry<ISoftDeletable> softDeletable 
      in EntriesToDelete(eventData.Context))
      {
        softDeletable.State = EntityState.Modified;
        softDeletable.Property(_isDeleted).CurrentValue = true;
        softDeletable.Property(_utcDeleted).CurrentValue = utcNow;
      }
    }
    return base.SavingChangesAsync(eventData, result, cancellationToken);

    IEnumerable<EntityEntry<ISoftDeletable>> EntriesToDelete(DbContext context)
    {
      return context
        .ChangeTracker
        .Entries<ISoftDeletable>()
        .Where(e => e.State is EntityState.Deleted);
    }
  }
}
