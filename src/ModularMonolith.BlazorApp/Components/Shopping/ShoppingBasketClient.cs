namespace ModularMonolith.BlazorApp.Components.Shopping;

public class ShoppingBasketClient
{
  private readonly HttpClient _httpClient;

  public ShoppingBasketClient(HttpClient httpClient) 
  => _httpClient = httpClient;

  public async Task<ShoppingBasketDTO?> GetShoppingBasket(
    int shoppingBasketId
  , CancellationToken cancellationToken = default)
  {
    HttpResponseMessage response =
    await _httpClient.GetAsync($"/{shoppingBasketId}", cancellationToken);
    _ = response.EnsureSuccessStatusCode();
    ShoppingBasketDTO? shoppingBasket =
      await response.Content.ReadFromJsonAsync<ShoppingBasketDTO>(cancellationToken);
    return shoppingBasket;
  }
}
