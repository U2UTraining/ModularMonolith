namespace ModularMonolith.BoundedContexts.Currencies.CommandHandlers;

internal sealed class UpdateCurrencyValueInEuroCommandHandler
: ICommandHandler<UpdateCurrencyValueInEuroCommand, Currency>
{
  private readonly ICurrencyRepository _currencyRepo;
  private readonly IIntegrationEventPublisher _publisher;

  public UpdateCurrencyValueInEuroCommandHandler(
    ICurrencyRepository currencyRepo
  , IIntegrationEventPublisher publisher)
  {
    _currencyRepo = currencyRepo;
    _publisher = publisher;
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
