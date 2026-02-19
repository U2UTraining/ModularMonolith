using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ModularMonolithEFCore.SoftDelete;

public static partial class EntityConfigurationExtensions
{
  /// <summary>
  /// Enables soft delete for this entity, entities get marked as deleted
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="entity"></param>
  /// <param name="isDeleted"></param>
  /// <param name="utcDeleted"></param>
  /// <returns></returns>
  public static EntityTypeBuilder<T> HasSoftDelete<T>(
    this EntityTypeBuilder<T> entity
  , string isDeleted = SoftDeleteable.IsDeleted
  , string utcDeleted = SoftDeleteable.UtcDeleted
  ) where T : class, ISoftDeletable
  {
    _ = entity
      .Property<DateTime?>(utcDeleted)
      .HasDefaultValueSql("GETUTCDATE()")
      .HasColumnOrder(int.MaxValue - 1);

    _ = entity
      .Property<bool>(isDeleted)
      .HasDefaultValue(false)
      .HasColumnOrder(int.MaxValue);

    // Uses a Query Filter to only list entities which have not been deleted.

    _ = entity.HasQueryFilter(e => !EF.Property<bool>(e, isDeleted));
    return entity;
  }

  public static OwnedNavigationBuilder<RootEntity, OwnedEntity> HasSoftDelete<RootEntity, OwnedEntity>(
  this OwnedNavigationBuilder<RootEntity, OwnedEntity> entity
  , string isDeleted = SoftDeleteable.IsDeleted
  , string utcDeleted = SoftDeleteable.UtcDeleted
  )
    where RootEntity
  : class
    where OwnedEntity 
  : class
  , ISoftDeletable
  {
    _ = entity
      .Property<DateTime?>(utcDeleted)
      .HasDefaultValueSql("GETUTCDATE()")
      .HasColumnOrder(int.MaxValue - 1);

    _ = entity
      .Property<bool>(isDeleted)
      .HasDefaultValue(false)
      .HasColumnOrder(int.MaxValue);

    // Owned entities are retrieved through the aggregate root, so we can use the
    // aggregate root query filter to also filter out owned entities which are marked as deleted.

    return entity;
  }
}
