using System.Text;
using System.Text.Json;

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
    await _httpClient.GetAsync($"{shoppingBasketId}", cancellationToken);
    _ = response.EnsureSuccessStatusCode();
    ShoppingBasketDTO? shoppingBasket =
      await response.Content.ReadFromJsonAsync<ShoppingBasketDTO>(cancellationToken);
    return shoppingBasket;
  }

  public async Task<int?> CreateShoppingBasket(
    CancellationToken cancellationToken = default
  )
  {
    using StringContent content =
      new StringContent(string.Empty, Encoding.UTF8, "application/json");
    HttpResponseMessage response =
    await _httpClient.PostAsync("", content, cancellationToken);
    _ = response.EnsureSuccessStatusCode();
    int? shoppingBasketId =
      await response.Content.ReadFromJsonAsync<int>(cancellationToken);
    return shoppingBasketId;
  }

  public async Task SelectBoardGame(
    int shoppingBasketId
  , int boardGameId
  , decimal priceInEuro
  , CancellationToken cancellationToken = default)
  {
    AddBoardGameToShoppingBasketDTO dto = new(
      ShoppingBasketId: shoppingBasketId
    , BoardGameId: boardGameId
    , PriceInEuro: priceInEuro
    );
    string jsonData =
      JsonSerializer.Serialize(dto);
    using var content =
      new StringContent(jsonData, Encoding.UTF8, "application/json");
    HttpResponseMessage response =
      await _httpClient.PutAsync("", content, cancellationToken);
    response.EnsureSuccessStatusCode();
  }
}
