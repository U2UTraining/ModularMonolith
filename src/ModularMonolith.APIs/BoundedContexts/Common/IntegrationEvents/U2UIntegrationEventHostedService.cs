namespace ModularMonolith.BoundedContexts.Common.IntegrationEvents;

/// <summary>
/// Hosted service to process integration events from a channel.
/// </summary>
public class U2UIntegrationEventHostedService
: BackgroundService
{
  private readonly Channel<IIntegrationEvent> _channel;
  private readonly U2UIntegrationEventProcessor _eventProcessor;
  private readonly IServiceProvider _serviceProvider;

  public U2UIntegrationEventHostedService(
    Channel<IIntegrationEvent> channel
  , U2UIntegrationEventProcessor eventProcessor
  , IServiceProvider serviceProvider
  )
  {
    _channel = channel;
    _eventProcessor = eventProcessor;
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(
    CancellationToken cancellationToken)
  {
    await foreach (IIntegrationEvent @event 
    in _channel.Reader.ReadAllAsync(cancellationToken))
    {
      // Process message
      await _eventProcessor.ProcessIntegrationEventAsync(@event, cancellationToken);
    }
  }
}
