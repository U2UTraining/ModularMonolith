using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

using OpenTelemetryDemo.ServiceDefaults.Meters;

namespace ModularMonolith.BlazorApp.Components.IntegrationEvents;

/// <summary>
/// Processes integration events by invoking the appropriate handlers.
/// Uses a different scope than the publisher, so that the publisher can continue
/// </summary>
public class U2UBlazorIntegrationEventProcessor
{
  private readonly IServiceProvider _serviceProvider;

  public U2UBlazorIntegrationEventProcessor(
    IServiceProvider serviceProvider
  )
  {
    _serviceProvider = serviceProvider;
  }

  public async ValueTask ProcessIntegrationEventAsync(
    IIntegrationEvent @event
  , CancellationToken cancellationToken = default)
  {
    Type serviceType = typeof(IIntegrationEventHandler<>)
      .MakeGenericType(@event.GetType());
    IEnumerable<object?> integrationEventHandlers =
      _serviceProvider.GetServices(serviceType: serviceType);
    if (integrationEventHandlers is not null && integrationEventHandlers.Any())
    {
      Func<object, object, CancellationToken, ValueTask> invoker =
        APIs.BoundedContexts.Common.DomainEvents.U2UDomainEventInvoker.Instance.GetInvoker(serviceType);
      foreach (var handler in from IIntegrationEventHandler? handler in integrationEventHandlers
                              where handler is not null
                              select handler)
      {
        await invoker(handler, @event, cancellationToken);
      }
    }
  }
}
