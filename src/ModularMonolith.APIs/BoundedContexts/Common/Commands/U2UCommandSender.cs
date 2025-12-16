
namespace ModularMonolith.APIs.BoundedContexts.Common.Commands;

using Azure;

using FluentValidation.Results;

using Invoker = Func<object, object, CancellationToken, Task<object>>;

using Validator = Func<
    object                    // IValidator<T>
  , object                    // command
  , CancellationToken
  , Task<FluentValidation.Results.ValidationResult>>;

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
    await TryValidateCommand<TResponse>(command, cancellationToken);
    Type commandHandlerType = typeof(ICommandHandler<,>)
      .MakeGenericType(@command.GetType(), typeof(TResponse));
    object handler = _serviceProvider.GetRequiredService(commandHandlerType);
    Invoker invoker
    = U2UCommandInvoker.Instance.GetInvoker(commandHandlerType);
    return (TResponse) await invoker(handler, command, cancellationToken);
  }

  public async ValueTask TryValidateCommand<TResponse>(
    ICommand<TResponse> command
  , CancellationToken cancellationToken)
  {
    Type validatorType = typeof(IValidator<>).MakeGenericType(@command.GetType());
    IValidator? validator = _serviceProvider.GetService(validatorType) as IValidator;
    if (validator is not null)
    {
      Validator validatorInvoker = U2UCommandInvoker.Instance.GetValidator(validatorType);
      ValidationResult result = await validatorInvoker(validator, command, cancellationToken);
      if (!result.IsValid)
      {
        //result.Errors.ErrorMessage
        throw new ArgumentException($"Command validation failed with {result.Errors}");
      }
    }
  }
}
