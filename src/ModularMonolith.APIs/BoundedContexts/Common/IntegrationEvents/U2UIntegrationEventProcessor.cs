namespace ModularMonolithBoundedContexts.Common.IntegrationEvents;

/// <summary>
/// Processes integration events by invoking the appropriate handlers.
/// Uses a different scope than the publisher, so that the publisher can continue
/// </summary>
public class U2UIntegrationEventProcessor
{
  private readonly IServiceProvider _serviceProvider;

  public U2UIntegrationEventProcessor(
    IServiceProvider serviceProvider
  )
  => _serviceProvider = serviceProvider;

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
      foreach (IIntegrationEventHandler? handler in integrationEventHandlers)
      {
        if (handler is not null)
        {
          await invoker(handler, @event, cancellationToken);
        }
      }
    }
  }
}
