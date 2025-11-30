using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.Currencies.CurrencyConversions;

/// <summary>
/// Convert an amount from one currency to another.
/// </summary>
public interface ICurrencyConverterService
{
  ValueTask<PositiveDecimal> ConvertAmountAsync(
    PositiveDecimal amount
  , CurrencyName fromCurrency
  , CurrencyName toCurrency
  , CancellationToken cancellationToken
  );
}
