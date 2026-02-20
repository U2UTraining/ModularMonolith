using System.Net.ServerSentEvents;

namespace ModularMonolith.APIs.BoundedContexts.Common.Endpoints;

//[Register(ServiceLifetime.Scoped, methodNameHint: "AddCommonEndpoints")]
public sealed class SubcribeToIntegrationEvents(
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
