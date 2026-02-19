namespace ModularMonolith.APIs.BoundedContexts.Currencies.Queries;

[Register(
  interfaceType: typeof(IQueryHandler<GetValueForCurrencyQuery, PositiveDecimal[]>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]
public class GetValueForCurrencyQueryHandler(CurrenciesDb db)
  : IQueryHandler<GetValueForCurrencyQuery, PositiveDecimal[]>
{
  public async Task<PositiveDecimal[]> HandleAsync(
    GetValueForCurrencyQuery query
  , CancellationToken cancellationToken = default)
  {
    if (query.FromCurrency == query.ToCurrency)
    {
      return query.Amounts;
    }
    decimal fromValueInEur = await ValueForCurrency(query.FromCurrency, cancellationToken);
    decimal toValueInEur = await ValueForCurrency(query.ToCurrency, cancellationToken);
    return query.Amounts
      .Select(amount => new PositiveDecimal((amount.Value * fromValueInEur) / toValueInEur ))
      .ToArray();
  }

  private async Task<decimal> ValueForCurrency(CurrencyName currencyName, CancellationToken cancellationToken)
  {
    decimal currency = await db.Currencies
      .Where(c => c.Id.Key == currencyName)
      .Select(c => c.ValueInEuro.Value)
      .FirstOrDefaultAsync(cancellationToken);
    return currency;
  }
}
