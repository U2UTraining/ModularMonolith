using FluentValidation.Results;

namespace ModularMonolith.APIs.BoundedContexts.Common.Commands;

using Invoker = Func<
  object                      // CommandHandler
, object                      // Command
, CancellationToken
, Task<object>>;

using Validator = Func<
    object                    // IValidator<T>
  , object                    // command
  , CancellationToken
  , Task<ValidationResult>>;

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

  private Validator CreateValidator(
    Type commandValidatorType)
  {
    Type commandType = commandValidatorType.GetGenericArguments()[0];

    // (validator, command, cancellationToken)
    // => validator.ValidateAsync(@command, cancellationToken)
    ParameterExpression validator =
      Expression.Parameter(typeof(object), "validator");
    ParameterExpression command =
      Expression.Parameter(typeof(object), "command");
    ParameterExpression cancellationToken =
      Expression.Parameter(typeof(CancellationToken), "cancellationToken");
    UnaryExpression validatorCast =
      Expression.Convert(validator, commandValidatorType);
    UnaryExpression commandCast =
      Expression.Convert(command, commandType);
    MethodInfo? validateAsyncMethod =
      commandValidatorType.GetMethod("ValidateAsync");
    MethodCallExpression method =
      Expression.Call(validatorCast, validateAsyncMethod!, commandCast, cancellationToken);
    //Expression<Func<object, object, CancellationToken, Task<object>>> lambda =
    //  Expression.Lambda<Func<object, object, CancellationToken, Task<object>>>(method, commandHandler, command, cancellationToken);

    Type lambdaType = typeof(Func<,,,>)
      .MakeGenericType(
        typeof(object)               // validator
      , typeof(object)               // command
      , typeof(CancellationToken)
      , typeof(Task<ValidationResult>)
      );
    var lambda = Expression.Lambda(lambdaType, method, validator, command, cancellationToken);
    Validator compiled = (Validator) lambda.Compile();

    // Wrap Task<TResult> as Task<object>
    return compiled;
  }

  private Dictionary<Type, Validator> _validators
    = new();

  public Validator GetValidator(
    Type validatorType)
  {
    if (_validators.TryGetValue(validatorType, out Validator? validator))
    {
      return validator;
    }
    validator = CreateValidator(validatorType);
    _validators[validatorType] = validator;
    return validator;
  }
}
