namespace ModularMonolith.BlazorApp.UIUpdates;

public sealed class UpdateClient
{
  private readonly HttpClient _httpClient;

  public UpdateClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<HttpResponseMessage> GetTokens(CancellationToken cancellationToken)
  {
    HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, "/ui_updates");
    //httpRequestMessage.SetBrowserResponseStreamingEnabled(true); // Blazor WASM

    // This requests server-sent-events
    httpRequestMessage.Headers.Add("Accept", "text/event-stream");  

    return await _httpClient.SendAsync(
      httpRequestMessage
    , HttpCompletionOption.ResponseHeadersRead
    , cancellationToken);
  }
}
