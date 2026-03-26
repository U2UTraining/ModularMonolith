namespace ModularMonolith.APIs.BoundedContexts.Currencies.Commands;

[Register(
  interfaceType: typeof(ICommandHandler<UpdateCurrencyValueInEuroCommand, Currency>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]
[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]

internal sealed class UpdateCurrencyValueInEuroCommandHandler
: ICommandHandler<UpdateCurrencyValueInEuroCommand, Currency>
{
  private readonly CurrenciesDb _db;
  private readonly IIntegrationEventPublisher _publisher;
  private readonly IOutboxSignal _outboxSignal;
  private readonly ILogger<UpdateCurrencyValueInEuroCommandHandler> _logger;

  public UpdateCurrencyValueInEuroCommandHandler(
    CurrenciesDb db
  , IIntegrationEventPublisher publisher
  , [FromKeyedServices(nameof(CurrenciesDb))] IOutboxSignal outboxSignal
  , ILogger<UpdateCurrencyValueInEuroCommandHandler> logger)
  {
    _db = db;
    _publisher = publisher;
    _outboxSignal = outboxSignal;
    _logger = logger;
  }

  public async Task<Currency> HandleAsync(
    UpdateCurrencyValueInEuroCommand request
  , CancellationToken cancellationToken = default)
  {
    Currency? currency = 
      await _db.Currencies
               .Where(c => c.Id == request.Name)
               .SingleOrDefaultAsync(cancellationToken);
    if (currency is not null)
    {
      PositiveDecimal oldValue = currency.ValueInEuro;
      currency.UpdateValueInEuro(request.NewValue);
      CurrencyHasChangedIntegrationEvent @event = new(
        EventId: Guid.NewGuid()
      , CurrencyName: currency.Id.Key.ToString()
      , OldValueInEuro: oldValue.Value
      , NewValueInEuro: currency.ValueInEuro.Value
      , CurrencyString: currency.ToEuroString()
      );
      await _db.SaveChangesAsync(@event, _outboxSignal, cancellationToken);

      CurrencyLogger.UpdateCurrencyValueInEuroCommandInvoked(_logger, DateTime.UtcNow, request);
      return currency;
    }
    throw new ArgumentException(
      message: $"Currency with name {request.Name} was not found."
    , paramName: nameof(request));
  }
}
