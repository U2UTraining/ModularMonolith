namespace ModularMonolithBoundedContexts.Common.Commands;

/// <summary>
/// ICommandHandler performs the requested command.
/// </summary>
/// <typeparam name="TCommand">
/// The Command object
/// </typeparam>
/// <typeparam name="TResponse">
/// The expected response type
/// </typeparam>
/// <remarks>
/// ICommandHandler should be registered in the DI container.
/// </remarks>
public interface ICommandHandler<TCommand, TResponse>
where TCommand 
: ICommand<TResponse>
{
  Task<TResponse> HandleAsync(
    TCommand command
  , CancellationToken cancellationToken = default);
}
