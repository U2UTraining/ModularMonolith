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
  private readonly ICurrencyRepository _currencyRepo;
  private readonly CurrenciesDb _db;
  private readonly IIntegrationEventPublisher _publisher;
  private readonly IOutboxSignal _outboxSignal;
  private readonly ILogger<UpdateCurrencyValueInEuroCommandHandler> _logger;

  public UpdateCurrencyValueInEuroCommandHandler(
    ICurrencyRepository currencyRepo
  , CurrenciesDb db
  , IIntegrationEventPublisher publisher
  , IOutboxSignal outboxSignal
  , ILogger<UpdateCurrencyValueInEuroCommandHandler> logger)
  {
    _currencyRepo = currencyRepo;
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
    //await _currencyRepo.GetCurrencyWithNameAsync(
    //    request.Name, cancellationToken);
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
      //OutboxExtensions.SendIntegrationEvent(_db, @event, _outboxSignal);
      await _db.SendIntegrationEvent(@event, _outboxSignal, cancellationToken);
      //await _currencyRepo.SaveChangesAsync(cancellationToken);
      //_outboxSignal.Signal();

      CurrencyLogger.UpdateCurrencyValueInEuroCommandInvoked(_logger, DateTime.UtcNow, request);

      //// Only trigger integration event after successful change
      //await _publisher.PublishIntegrationEventAsync(
      //  new CurrencyHasChangedIntegrationEvent(
      //    CurrencyName: currency.Id.Key.ToString()
      //  , OldValueInEuro: oldValue.Value
      //  , NewValueInEuro: currency.ValueInEuro.Value
      //  , CurrencyString: currency.ToEuroString()
      //)
      //, cancellationToken);
      return currency;
    }
    throw new ArgumentException(
      message: $"Currency with name {request.Name} was not found."
    , paramName: nameof(request));
  }
}
