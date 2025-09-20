namespace U2U.ModularMonolith.BoundedContexts.Common.DomainEvents;

using Invoker = Func<object, object, CancellationToken, ValueTask>;

// https://www.milanjovanovic.tech/blog/building-a-custom-domain-events-dispatcher-in-dotnet

/// <summary>
/// Publishes the domain event to all registered handlers.
/// </summary>
public class U2UDomainEventPublisher
: IDomainEventPublisher
{
  private readonly IServiceProvider _serviceProvider;

  public U2UDomainEventPublisher(
    IServiceProvider serviceProvider) 
  => _serviceProvider = serviceProvider;

  public async ValueTask PublishAsync(
    IDomainEvent @event
  , CancellationToken cancellationToken = default)
  {
    Type serviceType = typeof(IDomainEventHandler<>)
      .MakeGenericType(@event.GetType());
    IEnumerable<object?> domainEventHandlers =
      _serviceProvider.GetServices(serviceType: serviceType);
    if (domainEventHandlers is not null && domainEventHandlers.Any())
    {
      Invoker invoker =
        U2UDomainEventInvoker.Instance.GetInvoker(serviceType);
      foreach (IDomainEventHandler? handler in domainEventHandlers)
      {
        if (handler is not null)
        {
          await invoker(handler, @event, cancellationToken);
        }
      }
    }
  }
}
