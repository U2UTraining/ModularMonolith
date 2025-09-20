namespace U2U.ModularMonolith.BoundedContexts.Common.Queries;

using Invoker = Func<object, object, CancellationToken, Task<object>>;

public sealed class U2UQuerySender
: IQuerySender
{
  private readonly IServiceProvider _serviceProvider;

  public U2UQuerySender(IServiceProvider serviceProvider) 
  => _serviceProvider = serviceProvider;

  /// <summary>
  /// Execute the query handler for given query.
  /// There should only be one query handler registered 
  /// for a given query type.
  /// </summary>
  /// <typeparam name="TResponse">Response type</typeparam>
  /// <param name="query">Instance implementing IQuery<typeparamref name="TResponse"/>></param>
  /// <param name="cancellationToken"></param>
  /// <returns>Instance of <typeparamref name="TResponse"/></returns>
  /// <exception cref="NotImplementedException"></exception>
  public async Task<TResponse> AskAsync<TResponse>(
    IQuery<TResponse> query
  , CancellationToken cancellationToken = default)
  {
    Type queryHandlerType = typeof(IQueryHandler<,>)
      .MakeGenericType(query.GetType(), typeof(TResponse));
    object handler = _serviceProvider.GetRequiredService(queryHandlerType);
    Invoker invoker
    = U2UQueryInvoker.Instance.GetInvoker(queryHandlerType);
    return (TResponse)await invoker(handler, query, cancellationToken);
  }
}
