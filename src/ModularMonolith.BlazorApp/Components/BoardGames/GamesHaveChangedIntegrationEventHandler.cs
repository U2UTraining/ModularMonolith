using ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;
using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.BlazorApp.Components.BoardGames;

public class GamesHaveChangedIntegrationEventHandler(
  State _state
, IToastService _toastService
)
: IIntegrationEventHandler<GamesHaveChangedIntegrationEvent>
{
  public async ValueTask HandleAsync(
    GamesHaveChangedIntegrationEvent @event
  , CancellationToken cancellationToken)
  {
    if (_state.CurrentPage is BoardGamesPage boardGamesPage)
    {
      _toastService.ShowInfo(title: $"Prices have been updated");
    }
    await ValueTask.CompletedTask;
  }
}
