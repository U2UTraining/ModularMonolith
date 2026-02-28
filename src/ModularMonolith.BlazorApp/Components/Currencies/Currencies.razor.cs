using System.Text.Json;
using Microsoft.JSInterop;
using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;
using ModularMonolith.BlazorApp.Components.IntegrationEvents;

namespace ModularMonolith.BlazorApp.Components.Currencies;

public sealed partial class Currencies
{
  [Inject]
  public required IToastService ToastService
  {
    get; init;
  }

  [Inject]
  public required IDialogService dialogService
  {
    get; init;
  }

  [Inject]
  public required CurrencyClient CurrencyClient
  {
    get; init;
  }

  private IQueryable<CurrencyDto>? _currencies = null;

  private IJSObjectReference? _source;

  protected override async Task OnInitializedAsync()
  {
    await base.OnInitializedAsync();
    _currencies = await GetCurrenciesAsync();
    _source = await JSRuntime.InvokeAsync<IJSObjectReference>(
      "registerForSse"
    , DotNetObjectReference.Create(this));
  }


  internal async ValueTask<IQueryable<CurrencyDto>> GetCurrenciesAsync()
  {
    IEnumerable<CurrencyDto> result =
      await CurrencyClient.GetCurrenciesAsync();
    this.StateHasChanged();
    return result.AsQueryable();
  }

  private async ValueTask EditCurrency(CurrencyDto currency)
  {
    CurrencyEditViewModel tempCurrency = new(currency.CurrencyName, currency.ValueInEuro);
    DialogParameters parameters = new()
    {
      Height = "240px",
      Title = $"Edit currency's value",
      PreventDismissOnOverlayClick = true,
      PreventScroll = true,
    };

    try
    {
      IDialogReference dialog =
        await dialogService.ShowDialogAsync<CurrencyEditorDialog>(tempCurrency, parameters);
      DialogResult result = await dialog.Result;
      if (!result.Cancelled)
      {
        // Fail fast
        await CurrencyClient.UpdateCurrencyValue(
          new CurrencyDto(tempCurrency.Name, tempCurrency.ValueInEuro), default);
        //_currencies = await GetCurrenciesAsync();
        //ToastService.ShowSuccess(
        //   title: $"Currency {tempCurrency.Name} updated to {tempCurrency.ValueInEuro}."
        //);
      }
    }
    catch (Exception ex)
    {
      ToastService.ShowError(
        title: ex.Message
      );
    }
  }

  [Inject]
  public required IJSRuntime JSRuntime
  {
    get; init;
  }

  [Inject]
  public required U2UBlazorIntegrationEventProcessor IntegrationEventProcessor
  {
    get;init;
  }

  [JSInvokable]
  public async Task ProcessEvent(string @event, string eventType)
  {
    Type? type = Type.GetType(eventType, throwOnError:false);
    if (type is not null)
    {
      await this.InvokeAsync(async () =>
      {
        IIntegrationEvent? integrationEvent = JsonSerializer.Deserialize(@event, type) as IIntegrationEvent;
        if (integrationEvent is not null)
        {
          await IntegrationEventProcessor.ProcessIntegrationEventAsync(integrationEvent);
        }
      });
    }
  }
}
