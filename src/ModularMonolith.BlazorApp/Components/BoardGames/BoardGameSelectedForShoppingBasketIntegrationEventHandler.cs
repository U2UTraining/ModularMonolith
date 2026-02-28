using ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;
using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.BlazorApp.Components.BoardGames;

public class BoardGameSelectedForShoppingBasketIntegrationEventHandler(
  State state
, IToastService toastService)
  : IIntegrationEventHandler<BoardGameSelectedForShoppingBasketIntegrationEvent>
{
  public async ValueTask HandleAsync(
    BoardGameSelectedForShoppingBasketIntegrationEvent @event
  , CancellationToken cancellationToken)
  {
    if (state.CurrentPage is BoardGamesPage boardGamesPage)
    {
      if (state.ShoppingBasketId == @event.ShoppingBasketId)
      {
        toastService.ShowWarning(
          title: $"Game {@event.BoardGameName} was added to basket.");
      }
    }
  }
}
