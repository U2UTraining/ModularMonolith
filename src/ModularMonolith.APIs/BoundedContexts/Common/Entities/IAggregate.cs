namespace ModularMonolith.BoundedContexts.Common.Entities;

/// <summary>
/// Marker interface to mark an entity belonging to an aggregate 
/// with aggregate root.
/// </summary>
/// <typeparam name="ROOT">
/// This has to be an IAggregateRoot
/// </typeparam>
public interface IAggregate<ROOT>
where ROOT 
: IAggregateRoot
{ }
