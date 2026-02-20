using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

[Register(ServiceLifetime.Scoped, methodNameHint: "AddCommonServices")]
public class IntegrationEventService(
  ChannelMultiplexer<IIntegrationEvent> integrationEventMultiplexer)
{
  public async IAsyncEnumerable<SseItem<string>> SubscribeAsync(
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
  {
    Channel<IIntegrationEvent> channel =
      await integrationEventMultiplexer.SubscribeAsync(cancellationToken);
    //try
    //{
    await foreach (IIntegrationEvent ev in channel.Reader
      .ReadAllAsync(cancellationToken))
    {
      string data = JsonSerializer.Serialize(ev, ev.GetType());
      string eventType = $"{ev.GetType().FullName}, {ev.GetType().Assembly.GetName().Name}";
      yield return new SseItem<string>(data, eventType);
    }
    //}
    //catch
    //{
    //  await integrationEventMultiplexer.UnsubscribeAsync(channel);
    //}
  }
}
