using ServiceCollectionExtensions = ModularMonolith.APIs.BoundedContexts.UI.DI.ServiceCollectionExtensions;

namespace ModularMonolith.APIs.BoundedContexts.UI.EndPoints;

public static class UIEndPoints
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder WithUIEndpoints()
    {
      group.MapGet("/ui_updates",
        static ([FromKeyedServices(ServiceCollectionExtensions.UIUpdateEventStreamKey)] Channel<string> updateChannel
        , CancellationToken token) =>
        TypedResults.ServerSentEvents(
          updateChannel.Reader.ReadAllAsync(token),
          eventType: "ui_update")
      );
      return group;
    }
  }
}
