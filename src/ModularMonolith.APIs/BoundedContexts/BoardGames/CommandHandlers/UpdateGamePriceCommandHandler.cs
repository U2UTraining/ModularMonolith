using ModularMonolith.APIs.BoundedContexts.BoardGames.Commands;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.CommandHandlers;

internal sealed class UpdateGamePriceCommandHandler 
: ICommandHandler<UpdateGamePriceCommand, bool>
{
  public async Task<bool> HandleAsync(
    UpdateGamePriceCommand request
  , CancellationToken cancellationToken)
  {
    request.Game.SetPrice(request.PriceInEuro);
    return await Task.FromResult(true).ConfigureAwait(false);
  }
}
