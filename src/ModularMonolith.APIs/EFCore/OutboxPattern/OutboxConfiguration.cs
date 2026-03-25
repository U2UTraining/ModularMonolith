namespace ModularMonolith.APIs.EFCore.OutboxPattern;

/// <summary>
/// EF Core entity type configuration for <see cref="OutboxMessage"/>.
/// <para>
/// Centralising schema decisions here keeps them out of the entity class itself
/// and makes them easy to review alongside each other.
/// </para>
/// </summary>
public sealed class OutboxConfiguration
: IEntityTypeConfiguration<OutboxMessage>
{
  public void Configure(
    EntityTypeBuilder<OutboxMessage> outboxMessage)
  {
    _ = outboxMessage.ToTable("Outbox");

    // A clustered primary key keeps physically-adjacent rows sorted by insertion
    // order, which is the natural read order for the outbox processor.
    _ = outboxMessage
      .HasKey(ob => ob.Id)
      .IsClustered();

    // 1024 characters instead of the original 512 because assembly-qualified type
    // names (namespace + type + assembly + version + culture + public key token)
    // can easily exceed 512 characters for types in strongly-named assemblies.
    _ = outboxMessage
      .Property(ob => ob.EventType)
      .IsRequired()
      .HasMaxLength(1024);

    // 4096 characters covers most integration event payloads. Increase this limit
    // (or switch to MAX) if events with large collections are introduced.
    _ = outboxMessage
      .Property(ob => ob.Payload)
      .IsRequired()
      .HasMaxLength(4096);

    _ = outboxMessage
      .Property(ob => ob.UtcProcessed)
      ;

    // An index on UtcProcessed is necessary because the global query filter below
    // filters on this column. Without it the outbox processor would do a full
    // table scan on every poll as the table grows.
    _ = outboxMessage
      .HasIndex(ob => ob.UtcProcessed)
      ;

    _ = outboxMessage
      .HasAuditability()
      ;

    // The global query filter automatically restricts every query to unprocessed
    // messages, so callers never need to add a .Where(x => x.UtcProcessed == null)
    // themselves. Processed rows are effectively soft-deleted from the read path
    // while remaining available for auditing.
    outboxMessage
      .HasQueryFilter(ob => ob.UtcProcessed == null)
      ;
  }
}
