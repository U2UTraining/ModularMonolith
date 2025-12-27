using ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

namespace ModularMonolith.BlazorApp.Components.BoardGames;

public class BoardGamesClient
{
  private readonly HttpClient _httpClient;

  public BoardGamesClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="query"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <remarks>
  /// Here we cannot use the GET method, because we need to send the query in the body.
  /// Solution: Use the QUERY method.</remarks>
  public async Task<IEnumerable<GameDto>> GetGamesAsync(
    GetGamesQuery query
  , CancellationToken cancellationToken = default)
  {
    var content = JsonContent.Create(query);
    HttpResponseMessage response =
      await _httpClient.PostAsync("", content, cancellationToken);
    response.EnsureSuccessStatusCode();
    IEnumerable<GameDto>? games =
      await response.Content.ReadFromJsonAsync<IEnumerable<GameDto>>(cancellationToken);
    return games ?? [];
  }

  public async Task ApplyMegaDiscountAsync(
    CancellationToken cancellationToken = default)
  {
    HttpResponseMessage response =
      await _httpClient.PutAsync("apply-discount", null, cancellationToken);
    response.EnsureSuccessStatusCode();
  }
}
