namespace ModularMonolith.BlazorApp.Components.Currencies;

public class CurrencyClient
{
  private readonly HttpClient _httpClient;

  public CurrencyClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<IEnumerable<CurrencyDTO>> GetCurrenciesAsync(
    CancellationToken cancellationToken = default)
  {
    HttpResponseMessage response = 
      await _httpClient.GetAsync("currencies", cancellationToken);
    response.EnsureSuccessStatusCode();
    IEnumerable<CurrencyDTO>? currencies = 
      await response.Content.ReadFromJsonAsync<IEnumerable<CurrencyDTO>>(cancellationToken);
    return currencies ?? [];
  }

  public async Task<CurrencyDTO> UpdateCurrencyValue(
    CurrencyDTO currencyDTO
  , CancellationToken cancellationToken)
  {
    HttpResponseMessage response =
      await _httpClient.PutAsJsonAsync("currencies", currencyDTO, cancellationToken);
    response.EnsureSuccessStatusCode();
    CurrencyDTO? updatedCurrency =
      await response.Content.ReadFromJsonAsync<CurrencyDTO>();
    return updatedCurrency ?? currencyDTO;
  }
}
