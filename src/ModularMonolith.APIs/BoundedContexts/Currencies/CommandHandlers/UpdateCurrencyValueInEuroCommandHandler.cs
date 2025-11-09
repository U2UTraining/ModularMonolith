namespace ModularMonolith.APIs.BoundedContexts.Currencies.CommandHandlers;

internal sealed class UpdateCurrencyValueInEuroCommandHandler
: ICommandHandler<UpdateCurrencyValueInEuroCommand, Currency>
{
  private readonly ICurrencyRepository _currencyRepo;
  private readonly IIntegrationEventPublisher _publisher;
  private readonly ILogger<UpdateCurrencyValueInEuroCommandHandler> _logger;

  public UpdateCurrencyValueInEuroCommandHandler(
    ICurrencyRepository currencyRepo
  , IIntegrationEventPublisher publisher
  , ILogger<UpdateCurrencyValueInEuroCommandHandler> logger)
  {
    _currencyRepo = currencyRepo;
    _publisher = publisher;
    _logger = logger;
  }

  public async Task<Currency> HandleAsync(
    UpdateCurrencyValueInEuroCommand request
  , CancellationToken cancellationToken = default)
  {
    Currency? currency = 
      await _currencyRepo.GetCurrencyWithNameAsync(
        request.Name, cancellationToken);
    if (currency is not null)
    {
      PositiveDecimal oldValue = currency.ValueInEuro;
      currency.UpdateValueInEuro(request.NewValue);
      await _currencyRepo.SaveChangesAsync(cancellationToken);

      //_logger.LogInformation($"Updated currency {request.Name} to {request.NewValue} at {DateTime.UtcNow}");

      //_logger.LogInformation("Updated currency {currencyName} to {newValue} at {timestamp}", request.Name, request.NewValue, DateTime.UtcNow);

      CurrencyLogger.UpdateCurrencyValueInEuroCommandInvoked(_logger, DateTime.UtcNow, request);
     
      // Only trigger integration event after successful change
      await _publisher.PublishIntegrationEventAsync(
        new CurrencyHasChangedIntegrationEvent(
          CurrencyName: currency.Id.Key.ToString()
        , OldValueInEuro: oldValue.Value
        , NewValueInEuro: currency.ValueInEuro.Value
        , CurrencyString: currency.ToEuroString()
      )
      , cancellationToken);
      return currency;
    }
    throw new ArgumentException(
      message: $"Currency with name {request.Name} was not found."
    , paramName: nameof(request));
  }
}

internal sealed partial class CurrencyLogger {

  [LoggerMessage(
        EventId = 123
      , Level = LogLevel.Information
      , Message = "Updated currency at {timestamp}")]
  public static partial void UpdateCurrencyValueInEuroCommandInvoked(
        ILogger logger
      , DateTime timestamp 
      , [LogProperties] UpdateCurrencyValueInEuroCommand command);

}
