using Microsoft.AspNetCore.Components;

using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;
using ModularMonolith.APIs.BoundedContexts.Currencies.IntegrationEvents;

namespace ModularMonolith.BlazorApp.Components.Shopping;

internal sealed class CurrencyHasChangedIntegrationEventHandler
: IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>
{
  private readonly State _state;
  private readonly IToastService _toastService;

  public CurrencyHasChangedIntegrationEventHandler(
    State state
  , IToastService toastService
    )
  {
    _state = state;
    _toastService = toastService;
  }

  public async ValueTask HandleAsync(
    CurrencyHasChangedIntegrationEvent notification
  , CancellationToken cancellationToken)
  {
    if( _state.CurrentPage is ShoppingBasketPage shoppingBasketPage)
    {
    //await _state.Dispatcher.InvokeAsync(() =>
    //{
      _toastService.ShowWarning(
      title: $"Currency {notification.CurrencyName} updated to {notification.NewValueInEuro}.");
      //});

      await shoppingBasketPage.RefreshAsync();
      //await currencies.InvokeStateHasChangedAsync();
    }
  }
}
