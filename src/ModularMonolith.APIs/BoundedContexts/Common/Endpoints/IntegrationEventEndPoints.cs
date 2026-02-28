namespace ModularMonolith.APIs.BoundedContexts.Common.Endpoints;

public static class IntegrationEventEndPoints
{
  public static IEndpointRouteBuilder MapIntegrationEventEndpoints(
      this IEndpointRouteBuilder endpoints)
  {
    var unused = endpoints.MapGet("/integration-events",
      ([FromServices] SubscribeToIntegrationEvents handler
    , CancellationToken cancellationToken)
      => handler.ExecuteAsync(cancellationToken));
    return endpoints;
  }
}
