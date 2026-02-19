namespace ModularMonolith.APIs.BoundedContexts.Currencies.DomainEvents;

[Register(
  interfaceType: typeof(IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]
internal sealed class CurrencyValueInEuroHasChangedLoggingDomainEventHandler
: IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
{
  private readonly ILogger<CurrencyValueInEuroHasChangedLoggingDomainEventHandler> _logger;

  public CurrencyValueInEuroHasChangedLoggingDomainEventHandler(
    ICurrencyRepository repo
  , ILogger<CurrencyValueInEuroHasChangedLoggingDomainEventHandler> logger)
  {
    _logger = logger;
  }

  public async ValueTask HandleAsync(
    CurrencyValueInEuroHasChangedDomainEvent notification
  , CancellationToken cancellationToken)
  {
    _logger.LogInformation(
    """
    The Currency with name {CurrencyName} has changed conversion rate to {ValueInEuro}
    """
    , notification.CurrencyName
    , notification.NewValueInEuro
    );
    await Task.CompletedTask;
  }
}
