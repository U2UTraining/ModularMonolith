using ServiceCollectionExtensions = ModularMonolith.APIs.BoundedContexts.UI.DI.ServiceCollectionExtensions;

namespace ModularMonolith.APIs.BoundedContexts.UI.Commands;

internal sealed class UpdateUICommandHandler
  : ICommandHandler<UpdateUICommand, bool>
{
  private readonly Channel<string> _updateChannel;

  public UpdateUICommandHandler([FromKeyedServices(ServiceCollectionExtensions.UIUpdateEventStreamKey)] Channel<string> updateChannel)
  {
    _updateChannel = updateChannel;
  }

  public Task<bool> HandleAsync(UpdateUICommand command, CancellationToken cancellationToken = default)
  {
    return Task.FromResult(_updateChannel.Writer.TryWrite("UpdateUI"));
  }
}
