namespace ModularMonolith.APIs.BoundedContexts.Common.Queries;

using Invoker = Func<object, object, CancellationToken, Task<object>>;

/// <summary>
/// Generate the Invoker for an IQueryHandler<TQuery, TResponse>
/// and cache it.
/// </summary>
/// <Remarks>
/// This class is a Singleton.
/// </Remarks>
public sealed class U2UQueryInvoker
{
  private U2UQueryInvoker() { }

  public static U2UQueryInvoker Instance { get; } = new();

  private Dictionary<Type, Invoker> _invokers
    = new();

  private Invoker CreateInvoker(
    Type commandHandlerType)
  {
    // eventHandlerType is of type ICommandHandler<CommandType, ResultType>
    Type commandType = commandHandlerType.GetGenericArguments()[0];
    Type resultType = commandHandlerType.GetGenericArguments()[1];

    // (commandHandler, command, cancellationToken)
    // => commandHandler.HandleAsync(command, cancellationToken)
    ParameterExpression commandHandler =
      Expression.Parameter(typeof(object), "commandHandler");
    ParameterExpression command =
      Expression.Parameter(typeof(object), "command");
    ParameterExpression cancellationToken =
      Expression.Parameter(typeof(CancellationToken), "cancellationToken");
    UnaryExpression handlerCast =
      Expression.Convert(commandHandler, commandHandlerType);
    UnaryExpression cast =
      Expression.Convert(command, commandType);
    MethodInfo? handleAsyncMethod =
      commandHandlerType.GetMethod("HandleAsync");
    MethodCallExpression method =
      Expression.Call(handlerCast, handleAsyncMethod!, cast, cancellationToken);

    Type lambdaType = typeof(Func<,,,>)
      .MakeGenericType(
        typeof(object)
      , typeof(object)
      , typeof(CancellationToken)
      , typeof(Task<>).MakeGenericType(resultType));
    LambdaExpression lambda = Expression.Lambda(lambdaType, method, commandHandler, command, cancellationToken);
    Delegate compiled = lambda.Compile();

    // Wrap Task<TResult> as Task<object>
    return (object handler, object cmd, CancellationToken ct) =>
    {
      var task = (Task)compiled.DynamicInvoke(handler, cmd, ct)!;
      return task.ContinueWith(t =>
      {
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty?.GetValue(task)!;
      }, ct);
    };
    //return lambda.Compile();
  }

  public Invoker GetInvoker(
    Type invokerType)
  {
    if (_invokers.TryGetValue(invokerType, out Invoker? invoker))
    {
      return invoker;
    }
    invoker = CreateInvoker(invokerType);
    _invokers[invokerType] = invoker;
    return invoker;
  }
}
