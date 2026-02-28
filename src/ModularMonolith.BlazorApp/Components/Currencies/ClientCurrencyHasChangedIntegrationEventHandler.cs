using Microsoft.AspNetCore.Components;

using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;
using ModularMonolith.APIs.BoundedContexts.Currencies.IntegrationEvents;

namespace ModularMonolith.BlazorApp.Components.Currencies;

public sealed class ClientCurrencyHasChangedIntegrationEventHandler
: IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>
{
  private readonly State _state;
  private readonly IToastService _toastService;
  //private readonly Dispatcher _dispatcher;

  public ClientCurrencyHasChangedIntegrationEventHandler(
    State state
  , IToastService toastService
    //, Dispatcher dispatcher
    )
  {
    _state = state;
    _toastService = toastService;
    //_dispatcher = dispatcher;
  }

  public async ValueTask HandleAsync(
    CurrencyHasChangedIntegrationEvent notification
  , CancellationToken cancellationToken)
  {
    if( _state.CurrentPage is Currencies currencies)
    {
    //await _state.Dispatcher.InvokeAsync(() =>
    //{
      _toastService.ShowWarning(
      title: $"Currency {notification.CurrencyName} updated to {notification.NewValueInEuro}.");
      //});

      await currencies.GetCurrenciesAsync();
    }
  }
}
