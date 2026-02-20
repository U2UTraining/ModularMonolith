namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

public class ChannelMultiplexerHostedService(
  ChannelMultiplexer<IIntegrationEvent> multiplexer)
  : BackgroundService
{

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
    => multiplexer.MultiplexAsync(stoppingToken);

}
