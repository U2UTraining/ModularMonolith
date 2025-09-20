namespace ModularMonolith.BoundedContexts.Common.Queries;

/// <summary>
/// Interface to Ask Queries 
/// </summary>
/// <remarks>
/// There should only be one query handler registered 
/// for a given command type.
/// </remarks>
public interface IQuerySender
{
  Task<TResponse> AskAsync<TResponse>(
    IQuery<TResponse> query
  , CancellationToken cancellationToken = default);
}
