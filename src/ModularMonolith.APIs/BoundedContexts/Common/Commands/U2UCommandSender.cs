namespace ModularMonolithBoundedContexts.Common.Commands;

using Invoker = Func<object, object, CancellationToken, Task<object>>;

public sealed class U2UCommandSender
: ICommandSender
{
  private readonly IServiceProvider _serviceProvider;

  public U2UCommandSender(IServiceProvider serviceProvider) 
  => _serviceProvider = serviceProvider;

  /// <summary>
  /// Execute the command handler for given command.
  /// There should only be one command handler registered 
  /// for a given command type.
  /// </summary>
  /// <typeparam name="TResponse">Response type</typeparam>
  /// <param name="command">Instance implementing ICommand<typeparamref name="TResponse"/>></param>
  /// <param name="cancellationToken"></param>
  /// <returns>Instance of <typeparamref name="TResponse"/></returns>
  /// <exception cref="NotImplementedException"></exception>
  public async Task<TResponse> ExecuteAsync<TResponse>(
    ICommand<TResponse> command
  , CancellationToken cancellationToken = default)
  {
    Type commandHandlerType = typeof(ICommandHandler<,>)
      .MakeGenericType(@command.GetType(), typeof(TResponse));
    object handler = _serviceProvider.GetRequiredService(commandHandlerType);
    Invoker invoker
    = U2UCommandInvoker.Instance.GetInvoker(commandHandlerType);
    return (TResponse) await invoker(handler, command, cancellationToken);
  }
}
