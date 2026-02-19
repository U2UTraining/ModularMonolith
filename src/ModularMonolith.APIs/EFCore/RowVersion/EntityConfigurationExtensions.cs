namespace ModularMonolithEFCore.RowVersion;

public static class EntityConfigurationExtensions
{
  // ✅ Add rowversion to aggregates; handle 409s (Conflict).
  public static EntityTypeBuilder<T> HasRowVersion<T>(
    this EntityTypeBuilder<T> entity
  , string columnName = RowVersion.Column
  )
  where T 
  : class
  {
    entity
      .Property<byte[]>(columnName)
      .IsRowVersion()
      ;
    return entity;
  }

  // ✅ Add rowversion to aggregates; handle 409s (Conflict).
  public static OwnedNavigationBuilder<RootEntity, OwnedEntity> HasRowVersion<RootEntity, OwnedEntity>(
    this OwnedNavigationBuilder<RootEntity, OwnedEntity> entity
  , string columnName = RowVersion.Column
  )
    where RootEntity
  : class
    where OwnedEntity
  : class
  {
    entity
      .Property<byte[]>(columnName)
      .IsRowVersion()
      ;
    return entity;
  }
}
