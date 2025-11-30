namespace ModularMonolith.APIs.BoundedContexts.Common.Queries;

/// <summary>
/// An IQueryHandler performs an IQuery
/// </summary>
/// <typeparam name="TQuery">
/// The IQuery instance
/// </typeparam>
/// <typeparam name="TResponse">
/// The result of the query, see IQuery<typeparamref name="TResponse"/>
/// </typeparam>
/// <remarks>
/// IQueryHandler should be registered in the DI container.
/// </remarks>
public interface IQueryHandler<TQuery, TResponse>
where TQuery 
: IQuery<TResponse> 
{
  Task<TResponse> HandleAsync(
    TQuery query
  , CancellationToken cancellationToken = default);
}

