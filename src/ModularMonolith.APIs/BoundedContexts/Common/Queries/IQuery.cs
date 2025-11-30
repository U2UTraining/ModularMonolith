namespace ModularMonolith.APIs.BoundedContexts.Common.Queries;

/// <summary>
/// A IQuery fetches some data.
/// The actual fetching is done using an IQueryHandler
/// </summary>
/// <typeparam name="TResponse">
/// The response type for the query
/// </typeparam>
public interface IQuery<out TResponse> 
{ }
