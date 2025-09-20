namespace U2U.ModularMonolith.BoundedContexts.Currencies.DomainEventHandlers;

public sealed class CurrencyValueInEuroHasChangedDomainEventHandler
: IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
{
  private readonly ICurrencyRepository _repo;
  private readonly ILogger<CurrencyValueInEuroHasChangedLoggingDomainEventHandler> _logger;

  public CurrencyValueInEuroHasChangedDomainEventHandler(
    ICurrencyRepository repo
  , ILogger<CurrencyValueInEuroHasChangedLoggingDomainEventHandler> logger)
  {
    _repo = repo;
    _logger = logger;
  }

  public async ValueTask HandleAsync(
    CurrencyValueInEuroHasChangedDomainEvent notification
  , CancellationToken cancellationToken = default)
  {
    // Testing only...
    // See if this passes the changes that were made
    IQueryable<Currency> list = await _repo.GetAllCurrenciesAsync();
    int count = list.Count();
  }
}
