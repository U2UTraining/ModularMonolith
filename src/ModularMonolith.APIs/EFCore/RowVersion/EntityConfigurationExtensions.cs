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
}
