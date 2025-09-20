namespace ModularMonolith.BoundedContexts.Common.Commands;

/// <summary>
/// Interface to Execute Commands
/// </summary>
/// <remarks>
/// This will execute the ICommand using the ICommandHandler.
/// There should only be one command handler registered 
/// for a given command type.
/// </remarks>
public interface ICommandSender
{
  Task<TResponse> ExecuteAsync<TResponse>(
    ICommand<TResponse> command
  , CancellationToken cancellationToken = default);
}
