using System.Net.ServerSentEvents;

using Azure;

namespace ModularMonolith.BlazorApp.Components.IntegrationEvents;

public class SseClient(HttpClient httpClient)
{
  /// <summary>
  /// Subscribes to server-sent events from the integration events endpoint.
  /// </summary>
  /// <param name="endpoint">The relative or absolute URL of the SSE endpoint.</param>
  /// <param name="cancellationToken">Token to cancel the subscription.</param>
  /// <returns>An async enumerable of SSE events received from the server.</returns>
  /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
  public async IAsyncEnumerable<SseItem<string>> SubscribeToIntegrationEvents(
    string endpoint = "",
    [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
  {
    HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, endpoint);
    httpRequestMessage.Headers.Add("Accept", "text/event-stream");

    HttpResponseMessage response = await httpClient.SendAsync(
      httpRequestMessage,
      HttpCompletionOption.ResponseHeadersRead,
      cancellationToken);

    // Ensure the request was successful before attempting to read the stream
    response.EnsureSuccessStatusCode();

    await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
    
    // Enumerate and yield each server-sent event as it arrives
    await foreach (SseItem<string> sseEvent in SseParser.Create(stream).EnumerateAsync().WithCancellation(cancellationToken))
    {
      yield return sseEvent;
    }
  }
}
