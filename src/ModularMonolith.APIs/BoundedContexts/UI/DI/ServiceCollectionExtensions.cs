namespace ModularMonolith.APIs.BoundedContexts.UI.DI;

public static class ServiceCollectionExtensions
{
  public const string UIUpdateEventStreamKey = "UIUpdateEventStream";
  public static IHostApplicationBuilder AddUIUpdateEvents(
  this IHostApplicationBuilder builder)
  {
    builder.Services.AddUIUpdateEventStream();
    return builder;
  }

  private static IServiceCollection AddUIUpdateEventStream(this IServiceCollection services)
  {
    services.AddKeyedSingleton(
      UIUpdateEventStreamKey,
      Channel.CreateUnbounded<string>(new UnboundedChannelOptions
      {
        SingleReader = true,
        SingleWriter = false
      })
    );
    return services;
  }

}
