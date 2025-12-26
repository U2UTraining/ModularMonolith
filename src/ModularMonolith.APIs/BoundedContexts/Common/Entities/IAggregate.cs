namespace ModularMonolith.APIs.BoundedContexts.Common.Entities;

/// <summary>
/// Marker interface to mark an entity belonging to an aggregate 
/// with aggregate root.
/// </summary>
/// <typeparam name="ROOT">
/// This has to be an IAggregateRoot
/// </typeparam>
#pragma warning disable S2326 // Unused type parameters should be removed
public interface IAggregate<ROOT>
#pragma warning restore S2326 // Unused type parameters should be removed
where ROOT 
: IAggregateRoot
{ }
