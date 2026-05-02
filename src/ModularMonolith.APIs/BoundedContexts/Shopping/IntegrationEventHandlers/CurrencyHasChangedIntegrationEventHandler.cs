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

    // ================ REASONING ===================================================
    // We have a product that costs 10$ and dollar has exchange rate of 1.0
    // Now when we change the exchange rate to 2.0 (so value of $ has doubled)
    // Our product was 20 EUR and 20 DLR whan exchange rate was 1.0
    // With exchage rate of 2.0 the price of the product should become 10 DLR
    // Factor is equivalent to 1/factor of exchange rate 
    // Factor is 20 DLR * OldValueInEuro / NewValueInEuro
    // ==============================================================================

    // Example of running (re-usable) query in current bounded context
    List<ShoppingBasket> baskets =
      await _queryHandler.HandleAsync(ShoppingBasketsWithStateQuery.Open);

    CurrencyName currency = Currency.Parse(notification.CurrencyName);
    decimal factor = notification.OldValueInEuro / notification.NewValueInEuro;
    foreach (ShoppingBasket basket in baskets)
    {
      basket.SyncGamePrices(currency, factor);
    }
    await _db.SaveChangesAsync();

    _logger.LogInformation("Ending Integration event");
  }
}
