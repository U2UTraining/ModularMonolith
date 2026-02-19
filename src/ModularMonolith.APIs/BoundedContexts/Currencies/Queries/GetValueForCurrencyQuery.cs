namespace ModularMonolith.APIs.BoundedContexts.Currencies.Queries;

public record class GetValueForCurrencyQuery(
    PositiveDecimal[] Amounts
  , CurrencyName FromCurrency
  , CurrencyName ToCurrency
)
  : IQuery<PositiveDecimal[]>
{
}
