namespace ModularMonolith.APIs.BoundedContexts.Common.Queries;

/// <summary>
/// A IQuery fetches some data.
/// The actual fetching is done using an IQueryHandler
/// </summary>
/// <typeparam name="TResponse">
/// The response type for the query
/// </typeparam>
/// <remarks>
/// Queries are special, they do not change state, 
/// do not raise domain/integration events.
/// They do not require validation, authorization, etc.
/// </remarks>
public interface IQuery<out TResponse>
{ }
