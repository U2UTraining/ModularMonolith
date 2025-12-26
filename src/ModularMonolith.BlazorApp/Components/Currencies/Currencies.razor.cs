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

  protected override async Task OnInitializedAsync()
  {
    _currencies = await GetCurrenciesAsync();
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
}
