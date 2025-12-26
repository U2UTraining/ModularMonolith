namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

/// <summary>
/// Hosted service to process integration events from a channel.
/// </summary>
public class U2UIntegrationEventHostedService
: BackgroundService
{
  private readonly Channel<IIntegrationEvent> _channel;
  private readonly U2UIntegrationEventProcessor _eventProcessor;

  public U2UIntegrationEventHostedService(
    Channel<IIntegrationEvent> channel
  , U2UIntegrationEventProcessor eventProcessor
  , IServiceProvider serviceProvider
  )
  {
    _channel = channel;
    _eventProcessor = eventProcessor;
  }

  protected override async Task ExecuteAsync(
    CancellationToken stoppingToken)
  {
    await foreach (IIntegrationEvent @event 
    in _channel.Reader.ReadAllAsync(stoppingToken))
    {
      // Process message
      await _eventProcessor.ProcessIntegrationEventAsync(@event, stoppingToken);
    }
  }
}
