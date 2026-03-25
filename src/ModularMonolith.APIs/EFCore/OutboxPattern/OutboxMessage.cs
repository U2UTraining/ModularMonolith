using System.Text.Json;

namespace ModularMonolith.APIs.EFCore.OutboxPattern;

/// <summary>
/// Represents a single entry in the outbox table.
/// <para>
/// The outbox pattern solves the dual-write problem: without it, saving a domain
/// change and publishing an integration event are two separate operations that can
/// get out of sync (e.g. the app crashes between the two). By writing the event
/// into this table inside the same EF Core <c>SaveChangesAsync</c> call as the
/// domain change, both writes share one database transaction — the event is either
/// committed together with the domain state or rolled back entirely.
/// The <see cref="OutboxHostedService{DB}"/> then reads from this table and
/// publishes the event asynchronously.
/// </para>
/// </summary>
public sealed class OutboxMessage
: IHasOutbox
, IAuditability
{
  public OutboxMessage() { }

  [SetsRequiredMembers]
  public OutboxMessage(IIntegrationEvent @event)
  {
    // AssemblyQualifiedName (e.g. "MyApp.Events.OrderPlaced, MyApp, Version=1.0.0.0, ...")
    // is stored instead of just GetType().Name so that Type.GetType() can resolve the
    // exact concrete class during deserialization. A short name is not enough because
    // multiple assemblies could define a type with the same simple name.
    EventType = @event.GetType().AssemblyQualifiedName
      ?? throw new InvalidOperationException($"Cannot resolve assembly-qualified name for {nameof(@event)}.");

    // The concrete type is passed to the serializer so that all properties on the
    // concrete class are included. Passing typeof(IIntegrationEvent) would only
    // serialize the interface members (none), producing an empty JSON object.
    Payload = JsonSerializer.Serialize(@event, @event.GetType());
  }

  /// <summary>
  /// Deserializes the payload back into a concrete <see cref="IIntegrationEvent"/> using
  /// the stored <see cref="EventType"/> assembly-qualified name.
  /// Returns <c>null</c> if the type cannot be resolved or the JSON is invalid.
  /// </summary>
  public IIntegrationEvent? DeserializePayload()
  {
    // Type.GetType requires an assembly-qualified name to locate the type across
    // assemblies — this is why we stored AssemblyQualifiedName at write time.
    Type? type = Type.GetType(EventType);
    if (type is null)
    {
      return null;
    }

    // Deserialize into the concrete type, then cast back to the interface.
    // Using the non-generic overload avoids the need for a compile-time type parameter
    // when the type is only known at runtime.
    return JsonSerializer.Deserialize(Payload, type) as IIntegrationEvent;
  }

  public int Id
  {
    get; set;
  }

  /// <summary>Assembly-qualified type name of the event, used to reconstruct the concrete type on read.</summary>
  public required string EventType
  {
    get; set;
  }

  /// <summary>JSON-serialized event payload.</summary>
  public required string Payload
  {
    get; set;
  }

  /// <summary>Set to UTC time once the event has been successfully published. <c>null</c> means pending.</summary>
  public DateTime? UtcProcessed { get; set; } = default;
}
