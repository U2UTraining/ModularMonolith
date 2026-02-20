using OpenTelemetryDemo.ServiceDefaults.Meters;

namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

/// <summary>
/// Processes integration events by invoking the appropriate handlers.
/// Uses a different scope than the publisher, so that the publisher can continue
/// </summary>
public class U2UIntegrationEventProcessor
{
  private readonly IServiceProvider _serviceProvider;
  private readonly IntegrationEventsMetrics _metrics;

  public U2UIntegrationEventProcessor(
    IServiceProvider serviceProvider
  )
  {
    _serviceProvider = serviceProvider;
    _metrics = _serviceProvider.GetRequiredService<IntegrationEventsMetrics>();
  }

  public async ValueTask ProcessIntegrationEventAsync(
    IIntegrationEvent @event
  , CancellationToken cancellationToken = default)
  {
    using var scope = _serviceProvider.CreateAsyncScope();
    Type serviceType = typeof(IIntegrationEventHandler<>)
      .MakeGenericType(@event.GetType());
    IEnumerable<object?> integrationEventHandlers =
      scope.ServiceProvider.GetServices(serviceType: serviceType);
    if (integrationEventHandlers is not null && integrationEventHandlers.Any())
    {
      Func<object, object, CancellationToken, ValueTask> invoker =
        U2UDomainEventInvoker.Instance.GetInvoker(serviceType);
      foreach (var handler in from IIntegrationEventHandler? handler in integrationEventHandlers
                              where handler is not null
                              select handler)
      {
        try
        {
          await invoker(handler, @event, cancellationToken);
          _metrics.IncreaseIntegrationEventsCounter();
        }
        catch
        {
          _metrics.IncreaseIntegrationEventsErrorCounter();
        }
      }
    }
  }
}
