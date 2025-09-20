namespace U2U.ModularMonolith.BoundedContexts.Common.Commands;

/// <summary>
/// Commands ask to do something, like update a table.
/// The actual work is performed using an ICommandHandler
/// </summary>
/// <typeparam name="TResponse">
/// The response for the command
/// </typeparam>
public interface ICommand<TResponse>
{ }
