namespace U2U.ModularMonolith.BoundedContexts.Common.DomainEvents;

using Invoker = Func<object, object, CancellationToken, Task<object>>;

/// <summary>
/// Generate the Invoker for an ICommandHandler<TCommand, TResponse>
/// and cache it.
/// </summary>
/// <Remarks>
/// This class is a Singleton.
/// </Remarks>
public sealed class U2UCommandInvoker
{
  private U2UCommandInvoker() { }

  public static U2UCommandInvoker Instance { get; } = new();

  private Dictionary<Type, Invoker> _invokers 
    = new();

  private Invoker CreateInvoker(
    Type commandHandlerType)
  {
    // eventHandlerType is of type ICommandHandler<CommandType, ResultType>
    Type commandType = commandHandlerType.GetGenericArguments()[0];
    Type resultType = commandHandlerType.GetGenericArguments()[1];
    //Type valueTaskType = typeof(ValueTask<>).MakeGenericType(resultType);

    // (eventHandler, event, cancellationToken)
    // => eventHandler.HandleAsync(@event, cancellationToken)
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
    //Expression<Func<object, object, CancellationToken, Task<object>>> lambda =
    //  Expression.Lambda<Func<object, object, CancellationToken, Task<object>>>(method, commandHandler, command, cancellationToken);

    var lambdaType = typeof(Func<,,,>)
      .MakeGenericType(
        typeof(object)
      , typeof(object)
      , typeof(CancellationToken)
      , typeof(Task<>).MakeGenericType(resultType));
    var lambda = Expression.Lambda(lambdaType, method, commandHandler, command, cancellationToken);
    var compiled = lambda.Compile();

    // Wrap Task<TResult> as Task<object>
    return async (object handler, object cmd, CancellationToken ct) =>
    {
      var task = (Task)compiled.DynamicInvoke(handler, cmd, ct)!;
      await task.ConfigureAwait(false);
      PropertyInfo? resultProperty = task.GetType().GetProperty("Result");
      return resultProperty?.GetValue(task)!;
    };
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
