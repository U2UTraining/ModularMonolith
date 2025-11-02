namespace ModularMonolith.BlazorApp.Components.BoardGames;

public class PublishersClient
{
  private readonly HttpClient _httpClient;

  public PublishersClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<IEnumerable<PublisherDTO>> GetPublishersAsync(
    CancellationToken cancellationToken = default)
  {
    HttpResponseMessage response =
      await _httpClient.GetAsync("", cancellationToken);
    response.EnsureSuccessStatusCode();
    IEnumerable<PublisherDTO>? publishers =
      await response.Content.ReadFromJsonAsync<IEnumerable<PublisherDTO>>(cancellationToken);
    return publishers ?? [];
  }
}
