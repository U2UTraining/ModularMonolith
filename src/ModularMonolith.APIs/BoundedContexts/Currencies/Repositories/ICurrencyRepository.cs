namespace ModularMonolithBoundedContexts.Currencies.Repositories;

public interface ICurrencyRepository 
: IRepository<Currency>
{
  ValueTask<IQueryable<Currency>> GetAllCurrenciesAsync(
    CancellationToken cancellationToken = default
  );

  ValueTask<Currency?> GetCurrencyWithNameAsync(
    PK<CurrencyName> name
  , CancellationToken cancellationToken = default
  );

  ValueTask<Currency> UpdateCurrencyValue(
    PK<CurrencyName> currencyName
  , PositiveDecimal value
  , CancellationToken cancellationToken = default
  );
}
