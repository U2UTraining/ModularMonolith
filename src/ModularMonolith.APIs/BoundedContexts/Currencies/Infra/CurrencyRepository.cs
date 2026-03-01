namespace ModularMonolith.APIs.BoundedContexts.Currencies.Infra;

[Register(
  interfaceType: typeof(ICurrencyRepository)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]
internal sealed class CurrencyRepository
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
  public async ValueTask<List<Currency>> GetAllCurrenciesAsync(
    CancellationToken cancellationToken = default
  ) 
  => await DbContext.Currencies.AsNoTracking().ToListAsync(cancellationToken);

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
      return currency;
    }
    throw new ArgumentException(
      message: $"Currency with name {currencyName} was not found."
    , paramName: nameof(currencyName));
  }
}
