namespace ModularMonolith.BlazorApp.Components.Currencies;

public class CurrencyClient
{
  private readonly HttpClient _httpClient;

  public CurrencyClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync(
    CancellationToken cancellationToken = default)
  {
    HttpResponseMessage response = 
      await _httpClient.GetAsync("", cancellationToken);
    response.EnsureSuccessStatusCode();
    IEnumerable<CurrencyDto>? currencies = 
      await response.Content.ReadFromJsonAsync<IEnumerable<CurrencyDto>>(cancellationToken);
    return currencies ?? [];
  }

  public async Task<CurrencyDto> UpdateCurrencyValue(
    CurrencyDto currencyDTO
  , CancellationToken cancellationToken)
  {
    HttpResponseMessage response =
      await _httpClient.PutAsJsonAsync("", currencyDTO, cancellationToken);
    response.EnsureSuccessStatusCode();
    CurrencyDto? updatedCurrency =
      await response.Content.ReadFromJsonAsync<CurrencyDto>();
    return updatedCurrency ?? currencyDTO;
  }
}
