namespace ModularMonolith.BlazorApp.Components.BoardGames;

public class BoardGamesClient
{
  private readonly HttpClient _httpClient;

  public BoardGamesClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<IEnumerable<GameDto>> GetGamesAsync(
    CancellationToken cancellationToken = default)
  {
    HttpResponseMessage response =
      await _httpClient.GetAsync("", cancellationToken);
    response.EnsureSuccessStatusCode();
    IEnumerable<GameDto>? games =
      await response.Content.ReadFromJsonAsync<IEnumerable<GameDto>>(cancellationToken);
    return games ?? [];
  }
}
