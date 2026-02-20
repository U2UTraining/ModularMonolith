using System.Text.Json;

using BlazorSseClient;

using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.JSInterop;

using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;
using ModularMonolith.APIs.BoundedContexts.Currencies.IntegrationEvents;

namespace ModularMonolith.BlazorApp.Components.Currencies;

public sealed partial class Currencies
  : IAsyncDisposable
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

  //[Inject]
  //public required BlazorSseClient.ISseClient SseClient
  //{
  //  get; init;
  //}

  private IQueryable<CurrencyDto>? _currencies = null;

  private IJSObjectReference? _source;

  protected override async Task OnInitializedAsync()
  {
    _currencies = await GetCurrenciesAsync();

    _source = await JSRuntime.InvokeAsync<IJSObjectReference>(
      "registerForSse"
    , DotNetObjectReference.Create(this));

    //SseClient.Subscribe("integrationEvent", async (message) =>
    //{
    //  // Process message
    //  ToastService.ShowInfo(
    //    title: $"Currency update received: {message}"
    //  );
    //  StateHasChanged();
    //});

    //await SseClient.StartAsync(" https://localhost:7248/integration-events");
  }


  private async ValueTask<IQueryable<CurrencyDto>> GetCurrenciesAsync()
  {
    IEnumerable<CurrencyDto> result =
      await CurrencyClient.GetCurrenciesAsync();
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
        _currencies = await GetCurrenciesAsync();
        ToastService.ShowSuccess(
           title: $"Currency {tempCurrency.Name} updated to {tempCurrency.ValueInEuro}."
        );
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
    get; set;
  }

  [JSInvokable]
  public async Task ProcessEvent(string @event, string eventType)
  {

    string text = typeof(CurrencyHasChangedIntegrationEvent).FullName ?? string.Empty;
    bool x = ReferenceEquals(eventType, text);

    Type? type = Type.GetType(eventType, throwOnError:false);
    if (type is not null)
    {
      //CurrencyHasChangedIntegrationEvent x = new();

      IIntegrationEvent? integrationEvent = JsonSerializer.Deserialize(@event, type) as IIntegrationEvent;
      if (integrationEvent is not null)
      {
        ToastService.ShowInfo(
          title: $"Currency update received: {integrationEvent}"
        );
        _currencies = await GetCurrenciesAsync();
        StateHasChanged();
      }
    }

    //Console.WriteLine(@event);
  }

  public  async ValueTask DisposeAsync()
  {
    await JSRuntime.InvokeVoidAsync("unregisterForSse", _source);
  }
}
