namespace ModularMonolith.APIs.BoundedContexts.Shopping.IntegrationEventHandlers;

[Register(
  interfaceType: typeof(IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShopping")]
public sealed class CurrencyHasChangedIntegrationEventHandler
: IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>
{
  private readonly ILogger<CurrencyHasChangedIntegrationEventHandler> _logger;

  public CurrencyHasChangedIntegrationEventHandler(
    ILogger<CurrencyHasChangedIntegrationEventHandler> logger
  ) 
  => _logger = logger;

  public async ValueTask HandleAsync(CurrencyHasChangedIntegrationEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Beginning Integration event");
    _logger.LogInformation("Ending Integration event");
    await Task.CompletedTask;
  }
}
