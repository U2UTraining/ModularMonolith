namespace ModularMonolith.BoundedContexts.Currencies.CurrencyConversions;

internal sealed class CurrencyConverterService
: ICurrencyConverterService
{
  private readonly ICurrencyRepository _repo;

  public CurrencyConverterService(ICurrencyRepository repo)
  {
    _repo = repo;
  }

  public async ValueTask<PositiveDecimal> ConvertAmountAsync(
    PositiveDecimal amount
  , CurrencyName fromCurrency
  , CurrencyName toCurrency
  , CancellationToken cancellationToken
  )
  {
    if (fromCurrency == toCurrency)
    {
      // Amount is already in expected currency
      return amount;
    }
    decimal fromValueInEur = await ValueForCurrency(fromCurrency, cancellationToken);
    decimal toValueInEur = await ValueForCurrency(toCurrency, cancellationToken);
    decimal fromInEur = amount.Value * fromValueInEur;
    decimal fromInCurrency = fromInEur / toValueInEur;
    return new PositiveDecimal(fromInCurrency);
  }

  private async ValueTask<decimal> ValueForCurrency(CurrencyName currencyName, CancellationToken cancellationToken)
  {
    Currency? currency = await _repo.GetCurrencyWithNameAsync(new PK<CurrencyName>(currencyName), cancellationToken);
    if (currency is null)
    {
      throw new ArgumentException(
        message: $"Unknown currency {currencyName}"
      , paramName: nameof(currencyName));
    }
    return currency.ValueInEuro.Value;
  }
}

