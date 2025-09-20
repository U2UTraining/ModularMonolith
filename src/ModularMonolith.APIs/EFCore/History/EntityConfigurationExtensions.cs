namespace U2U.ModularMonolith.EFCore.History;

public static partial class EntityConfigurationExtensions
{
  /// <summary>
  /// Adds two columns to the entity to keep track of changes
  /// </summary>
  /// <typeparam name="T">the entity</typeparam>
  /// <param name="entity">EntityTypeBuilder</param>
  /// <param name="created">Name of the column to hold the created timestamp</param>
  /// <param name="modified">Name of the column to hold the modified timestamp</param>
  /// <returns></returns>
  public static EntityTypeBuilder<T> HasHistory<T>(
    this EntityTypeBuilder<T> entity
  , string created = History.UtcCreated
  , string modified = History.UtcModified)
    where T 
  : class
  , IHistory
  {
    entity
      .Property<DateTime>(created)
      .HasDefaultValueSql("GETUTCDATE()") // Ensure the correct namespace is included  
      .HasColumnOrder(int.MaxValue - 2)
      .Metadata.SetDefaultValueSql("GETUTCDATE()")
      ; // Fix for CS1061  

    entity
       .Property<DateTime>(modified)
      .HasDefaultValueSql("GETUTCDATE()")
      .HasColumnOrder(int.MaxValue - 3) 
      // Ensure the correct namespace is included  
      .Metadata.SetDefaultValueSql("GETUTCDATE()")
      ; // Fix for CS1061  

    return entity;
  }
}
