namespace ModularMonolith.BlazorApp.Components.BoardGames;

public class BoardGamesClient
{
  private readonly HttpClient _httpClient;

  public BoardGamesClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<IEnumerable<GameDTO>> GetGamesAsync(
    CancellationToken cancellationToken = default)
  {
    HttpResponseMessage response =
      await _httpClient.GetAsync("games", cancellationToken);
    response.EnsureSuccessStatusCode();
    IEnumerable<GameDTO>? games =
      await response.Content.ReadFromJsonAsync<IEnumerable<GameDTO>>(cancellationToken);
    return games ?? [];
  }
}
