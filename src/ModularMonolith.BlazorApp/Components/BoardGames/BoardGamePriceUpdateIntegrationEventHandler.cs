using ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;
using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.BlazorApp.Components.BoardGames;

public sealed class BoardGamePriceUpdateIntegrationEventHandler(
  State state
, IToastService toastService)
: IIntegrationEventHandler<BoardGamePriceUpdateIntegrationEvent>
{
  public async ValueTask HandleAsync(
    BoardGamePriceUpdateIntegrationEvent @event
  , CancellationToken cancellationToken)
  {
    if (state.CurrentPage is BoardGamesPage boardGamesPage)
    {
      await boardGamesPage.RefreshBoardGamesAsync();
    }
    toastService.ShowWarning(title: $"Game {@event.Name} was added to basket.");
  }
}

