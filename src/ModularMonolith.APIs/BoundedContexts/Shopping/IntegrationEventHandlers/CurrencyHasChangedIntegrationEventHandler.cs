namespace ModularMonolith.APIs.BoundedContexts.Shopping.IntegrationEventHandlers;

[Register(
  interfaceType: typeof(IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShoppingServices")]
public sealed class CurrencyHasChangedIntegrationEventHandler
: IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>
{
  private readonly ILogger<CurrencyHasChangedIntegrationEventHandler> _logger;
  private readonly ShoppingDb _db;

  public CurrencyHasChangedIntegrationEventHandler(
    ILogger<CurrencyHasChangedIntegrationEventHandler> logger
  , ShoppingDb db
  )
  {
    _logger = logger;
    _db = db;
  }

  public async ValueTask HandleAsync(CurrencyHasChangedIntegrationEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Beginning Integration event");

    // TODO: Update the prices of the products in the shopping baskets. This is just an example, in a real application you would probably want to do this in a more efficient way, e.g. by using a stored procedure or by using a background job.
    // Find all baskets that have prices in the old currency and update them to the new currency. This is just an example, in a real application you would probably want to do this in a more efficient way, e.g. by using a stored procedure or by using a background job.
    var query = await _db.Baskets.ToListAsync(cancellationToken);
    int nr = query.Count;

    _logger.LogInformation("Ending Integration event");
    await Task.CompletedTask;
  }
}
