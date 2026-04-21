namespace ModularMonolith.APIs.BoundedContexts.Currencies.Queries;

[Register(
  serviceType: typeof(IQueryHandler<GetValueForCurrencyQuery, PositiveDecimal[]>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]
internal sealed class GetValueForCurrencyQueryHandler(CurrenciesDb db)
  : IQueryHandler<GetValueForCurrencyQuery, PositiveDecimal[]>
{
  public async Task<PositiveDecimal[]> HandleAsync(
    GetValueForCurrencyQuery query
  , CancellationToken cancellationToken = default)
  {
    //if (query.FromCurrency == query.ToCurrency)
    //{
    //  return query.Amounts;
    //}

    // Fetch both exchange rates in a single database round-trip
    List<Currency> currencies = await db.Currencies
      //.Where(c => c.Id == query.FromCurrency || c.Id == query.ToCurrency)
      .ToListAsync(cancellationToken);

    decimal fromValueInEur = currencies
      .First(c => c.Id.Key == query.FromCurrency)
      .ValueInEuro.Value;

    decimal toValueInEur = currencies
      .First(c => c.Id.Key == query.ToCurrency)
      .ValueInEuro.Value;

    return query.Amounts
      .Select(amount => new PositiveDecimal((amount.Value * fromValueInEur) / toValueInEur ).Rounded())
      .ToArray();
  }
}
