namespace ModularMonolith.BoundedContexts.Currencies.Infra;

public sealed class CurrencyRepository
: Repository<Currency, CurrenciesDb>
, ICurrencyRepository
{
  public CurrencyRepository(
    CurrenciesDb db
  , IDomainEventPublisher publisher)
  : base(db, publisher)
  { }

  /// <summary>
  /// Get list of Currencies
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns>List of untracked currencies</returns>
  /// <remarks>This method uses good old LINQ</remarks>
  public async ValueTask<IQueryable<Currency>> GetAllCurrenciesAsync(
    CancellationToken cancellationToken = default
  )
  {
    return await ValueTask.FromResult(DbContext.Currencies);
      //.AsNoTracking()
      //.ToListAsync(cancellationToken);
  }

  /// <summary>
  /// Get Currency by name
  /// </summary>
  /// <param name="name">name</param>
  /// <param name="cancellationToken"></param>
  /// <returns>Untracked Currency</returns>
  /// <remarks>This method uses SQL for performance</remarks>
  //public async ValueTask<Currency?> GetCurrencyWithNameAsync(
  //  CurrencyName name
  //, CancellationToken cancellationToken = default)
  //{
  //  Currency? result = await DbContext.Currencies.FromSqlInterpolated<Currency>(
  //    $$"""
  //    SELECT Name, ValueInEuro
  //    FROM [currencies].[Currencies] 
  //    WHERE Name = '{{name}}'
  //    """)
  //  .AsNoTracking()
  //  .SingleOrDefaultAsync(cancellationToken);
  //  return result;
  //}

  public async ValueTask<Currency?> GetCurrencyWithNameAsync(
    PK<CurrencyName> name
  , CancellationToken cancellationToken = default)
  {
    Currency? result = await DbContext.Currencies
                                     .Where(c => c.Id == name)
                                      //.Where(curr => EF.Property<PK<CurrencyName>>(curr, "Id") == name)
                                      .SingleOrDefaultAsync(cancellationToken);
    //Currency? result = await DbContext.Currencies
    //                                  .SingleOrDefaultAsync(c => c.Name == name, cancellationToken);

    return result;
  }

  public async ValueTask<Currency> UpdateCurrencyValue(
  PK<CurrencyName> currencyName
, PositiveDecimal value
, CancellationToken cancellationToken = default)
  {
    Currency? currency =
      await GetCurrencyWithNameAsync(currencyName, cancellationToken);
    if (currency is not null)
    {
      currency.UpdateValueInEuro(value);
      await SaveChangesAsync(cancellationToken);
      // Only trigger integration event after successful change
      //await _publisher.PublishIntegrationEventAsync(
      //  new CurrencyHasChangedIntegrationEvent(
      //    currency.Id.Key.ToString()
      //  , currency.ValueInEuro.Value
      //  , currency.ToEuroString())
      //, cancellationToken);
      return currency;
    }
    throw new ArgumentException(
      message: $"Currency with name {currencyName} was not found."
    , paramName: nameof(currencyName));
  }
}
