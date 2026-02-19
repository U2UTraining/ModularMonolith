namespace ModularMonolith.APIs.BoundedContexts.Currencies.DomainEvents;

[Register(
  interfaceType: typeof(IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]
internal sealed class CurrencyValueInEuroHasChangedDomainEventHandler
: IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
{
  private readonly ICurrencyRepository _repo;

  public CurrencyValueInEuroHasChangedDomainEventHandler(
    ICurrencyRepository repo
  , ILogger<CurrencyValueInEuroHasChangedDomainEventHandler> logger)
  {
    _repo = repo;
  }

  public async ValueTask HandleAsync(
    CurrencyValueInEuroHasChangedDomainEvent notification
  , CancellationToken cancellationToken)
  {
    // Testing only...
    // See if this passes the changes that were made
    List<Currency> list = await _repo.GetAllCurrenciesAsync();
    _ = list.Count;
  }
}
