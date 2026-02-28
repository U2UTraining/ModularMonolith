using System.Net.ServerSentEvents;

namespace ModularMonolith.APIs.BoundedContexts.Common.Endpoints;

public sealed class SubscribeToIntegrationEvents(
  IntegrationEventService integrationEventService)
{
  public ServerSentEventsResult<SseItem<string>> ExecuteAsync(
    CancellationToken cancellationToken)
  {
    return TypedResults.ServerSentEvents(
      integrationEventService.SubscribeAsync(cancellationToken)
    , eventType: "integrationEvent"
    );
  }
}
