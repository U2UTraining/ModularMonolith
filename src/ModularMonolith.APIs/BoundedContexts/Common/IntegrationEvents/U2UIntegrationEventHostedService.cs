using System.Net.ServerSentEvents;

namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

/// <summary>
/// Hosted service to process integration events from a channel.
/// </summary>
public class U2UIntegrationEventHostedService
: BackgroundService
{
  private readonly ChannelMultiplexer<IIntegrationEvent> _channelMultiplexer;
  private readonly U2UIntegrationEventProcessor _eventProcessor;

  public U2UIntegrationEventHostedService(
    ChannelMultiplexer<IIntegrationEvent> channelMultiplexer
  , U2UIntegrationEventProcessor eventProcessor
  )
  {
    _channelMultiplexer = channelMultiplexer;
    _eventProcessor = eventProcessor;
  }

  protected override async Task ExecuteAsync(
    CancellationToken cancellationToken)
  {
    Channel<IIntegrationEvent> channel =
      await _channelMultiplexer.SubscribeAsync(cancellationToken);
    await foreach (IIntegrationEvent @event in channel.Reader.ReadAllAsync() )
    {
      // Process message
      await _eventProcessor.ProcessIntegrationEventAsync(@event, cancellationToken);
    }
  }
}

//public class U2UIntegrationEventHostedService
//: BackgroundService
//{
//  private readonly Channel<IIntegrationEvent> _channel;
//  private readonly U2UIntegrationEventProcessor _eventProcessor;

//  public U2UIntegrationEventHostedService(
//    Channel<IIntegrationEvent> channel
//  , U2UIntegrationEventProcessor eventProcessor
//  )
//  {
//    _channel = channel;
//    _eventProcessor = eventProcessor;
//  }

//  protected override async Task ExecuteAsync(
//    CancellationToken stoppingToken)
//  {
//    await foreach (IIntegrationEvent @event
//    in _channel.Reader.ReadAllAsync(stoppingToken))
//    {
//      // Process message
//      await _eventProcessor.ProcessIntegrationEventAsync(@event, stoppingToken);
//    }
//  }
//}
