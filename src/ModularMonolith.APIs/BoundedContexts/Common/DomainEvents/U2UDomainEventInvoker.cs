namespace ModularMonolith.BoundedContexts.Common.DomainEvents;

using Invoker = Func<object, object, CancellationToken, ValueTask>;

public sealed class U2UDomainEventInvoker
{
  private U2UDomainEventInvoker() { }

  public static U2UDomainEventInvoker Instance { get; } = new();

  private Dictionary<Type, Invoker> _invokers 
    = new();

  private Invoker CreateInvoker(
    Type eventHandlerType)
  {
    // eventHandlerType is of type IDomainEventHandler<EventType>
    Type eventType = eventHandlerType.GetGenericArguments()[0];

    // (eventHandler, event, cancellationToken)
    // => eventHandler.HandleAsync(@event, cancellationToken)
    ParameterExpression eventHandler =
      Expression.Parameter(typeof(object), "eventHandler");
    ParameterExpression @event =
      Expression.Parameter(typeof(object), "event");
    ParameterExpression cancellationToken =
      Expression.Parameter(typeof(CancellationToken), "cancellationToken");
    UnaryExpression handlerCast =
      Expression.Convert(eventHandler, eventHandlerType);
    UnaryExpression cast =
      Expression.Convert(@event, eventType);
    MethodInfo? handleAsyncMethod =
      eventHandlerType.GetMethod("HandleAsync");
    MethodCallExpression method =
      Expression.Call(handlerCast, handleAsyncMethod!, cast, cancellationToken);
    var lambda =
      Expression.Lambda<Invoker>(method, eventHandler, @event, cancellationToken);
    return lambda.Compile();
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
