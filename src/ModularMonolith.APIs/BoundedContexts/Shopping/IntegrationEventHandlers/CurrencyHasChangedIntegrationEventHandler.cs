namespace ModularMonolith.APIs.BoundedContexts.Shopping.IntegrationEventHandlers;

[Register(
  serviceType: typeof(IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShoppingServices")]
internal sealed class CurrencyHasChangedIntegrationEventHandler
: IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>
{
  private readonly ILogger<CurrencyHasChangedIntegrationEventHandler> _logger;
  private readonly IQueryHandler<ShoppingBasketsWithStateQuery, List<ShoppingBasket>> _queryHandler;
  private readonly ShoppingDb _db;

  public CurrencyHasChangedIntegrationEventHandler(
    ILogger<CurrencyHasChangedIntegrationEventHandler> logger
  , IQueryHandler<ShoppingBasketsWithStateQuery, List<ShoppingBasket>> queryHandler
  , ShoppingDb db
  )
  {
    _logger = logger;
    _queryHandler = queryHandler;
    _db = db;
  }

  public async ValueTask HandleAsync(CurrencyHasChangedIntegrationEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Beginning Integration event");

    // TODO: Update the prices of the products in the shopping baskets.
    // This is just an example, in a real application you would probably
    // want to do this in a more efficient way, e.g. by using a stored procedure
    // or by using a background job.

    List<ShoppingBasket> baskets =
      await _queryHandler.HandleAsync(ShoppingBasketsWithStateQuery.Open);

    CurrencyName currency = Currency.Parse(notification.CurrencyName);
    decimal factor = notification.NewValueInEuro / notification.OldValueInEuro;
    foreach (ShoppingBasket basket in baskets)
    {
      basket.SyncGamePrices(currency, factor);
    }
    await _db.SaveChangesAsync();

    _logger.LogInformation("Ending Integration event");
  }
}
