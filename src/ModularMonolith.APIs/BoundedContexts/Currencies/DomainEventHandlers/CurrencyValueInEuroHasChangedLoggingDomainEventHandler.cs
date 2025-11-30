using ModularMonolith.APIs.BoundedContexts.Common.DomainEvents;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.DomainEventHandlers;

public sealed class CurrencyValueInEuroHasChangedLoggingDomainEventHandler
: IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
{
  private readonly ICurrencyRepository _repo;
  private readonly ILogger<CurrencyValueInEuroHasChangedLoggingDomainEventHandler> _logger;

  public CurrencyValueInEuroHasChangedLoggingDomainEventHandler(
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
    _logger.LogInformation(
    """
    The Currency with name {currencyName} has changed conversion rate to {ValueInEuro}
    """
    , notification.CurrencyName
    , notification.NewValueInEuro
    );
    await Task.CompletedTask;
  }
}
