namespace ModularMonolith.BlazorApp.Components.BoardGames;

public class PublishersClient
{
  private readonly HttpClient _httpClient;

  public PublishersClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<IEnumerable<PublisherDto>> GetPublishersAsync(
    CancellationToken cancellationToken = default)
  {
    HttpResponseMessage response =
      await _httpClient.GetAsync("", cancellationToken);
    response.EnsureSuccessStatusCode();
    IEnumerable<PublisherDto>? publishers =
      await response.Content.ReadFromJsonAsync<IEnumerable<PublisherDto>>(cancellationToken);
    return publishers ?? [];
  }

  public async Task<PublisherWithGamesDto?> GetPublisherWithGamesAsync(
    int publisherId
  , CancellationToken cancellationToken = default)
  {
    HttpResponseMessage response =
      await _httpClient.GetAsync($"{publisherId}", cancellationToken);
    response.EnsureSuccessStatusCode();
    PublisherWithGamesDto? publisher =
      await response.Content.ReadFromJsonAsync<PublisherWithGamesDto>(cancellationToken);
    return publisher;
  }
}
