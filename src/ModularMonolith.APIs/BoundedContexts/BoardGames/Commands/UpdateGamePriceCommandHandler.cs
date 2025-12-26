namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Commands;

internal sealed class UpdateGamePriceCommandHandler 
: ICommandHandler<UpdateGamePriceCommand, bool>
{
  public async Task<bool> HandleAsync(
    UpdateGamePriceCommand request
  , CancellationToken cancellationToken = default)
  {
    request.Game.SetPrice(request.PriceInEuro);
    return await Task.FromResult(true).ConfigureAwait(false);
  }
}
