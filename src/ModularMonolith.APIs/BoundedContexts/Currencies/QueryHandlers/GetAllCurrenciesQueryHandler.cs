namespace ModularMonolith.APIs.BoundedContexts.Currencies.QueryHandlers;

internal sealed class GetAllCurrenciesQueryHandler
: IQueryHandler<GetCurrenciesQuery, IQueryable<Currency>>
{
  private readonly ICurrencyRepository _repo;

  public GetAllCurrenciesQueryHandler(ICurrencyRepository repo)
  => _repo = repo;

  public async Task<IQueryable<Currency>> HandleAsync(
    GetCurrenciesQuery request
  , CancellationToken cancellationToken = default)
  {
    IQueryable<Currency> result =
      await _repo.GetAllCurrenciesAsync(cancellationToken);
    return result;
  }
}

internal sealed class GetAllCurrenciesQueryHandler2
: IQueryHandler<GetCurrenciesQuery, IQueryable<Currency>>
{
  private readonly CurrenciesDb _db;

  public GetAllCurrenciesQueryHandler2(CurrenciesDb db)
  {
    _db = db;
  }

  public async Task<IQueryable<Currency>> HandleAsync(
    GetCurrenciesQuery request
  , CancellationToken cancellationToken = default)
    => await ValueTask.FromResult(_db.Currencies);
}


internal sealed class GetAllCurrenciesQueryHandler3
: IQueryHandler<GetCurrenciesQuery, IQueryable<Currency>>
{
  private readonly IDbContextFactory<CurrenciesDb> _dbFactory;

  public GetAllCurrenciesQueryHandler3(IDbContextFactory<CurrenciesDb> dbFactory)
  {
    _dbFactory = dbFactory;
  }

  public async Task<IQueryable<Currency>> HandleAsync(
    GetCurrenciesQuery request
  , CancellationToken cancellationToken = default)
  {
    CurrenciesDb db =
      await _dbFactory.CreateDbContextAsync(cancellationToken);
    return db.Currencies;
  }
}
