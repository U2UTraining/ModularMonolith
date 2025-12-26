namespace ModularMonolith.APIs.BoundedContexts.Currencies.Queries;

/// <summary>
/// Get all currencies query handler using repository
/// </summary>
internal sealed class GetAllCurrenciesQueryHandler
: IQueryHandler<GetCurrenciesQuery, List<Currency>>
{
  private readonly ICurrencyRepository _repo;

  public GetAllCurrenciesQueryHandler(ICurrencyRepository repo)
  => _repo = repo;

  public async Task<List<Currency>> HandleAsync(
    GetCurrenciesQuery request
  , CancellationToken cancellationToken = default)
  {
    List<Currency> result =
      await _repo.GetAllCurrenciesAsync(cancellationToken);
    return result;
  }
}

/// <summary>
/// Get all currencies query handler using DbContext directly
/// </summary>
internal sealed class GetAllCurrencies2QueryHandler
: IQueryHandler<GetCurrenciesQuery, List<Currency>>
{
  private readonly CurrenciesDb _db;

  public GetAllCurrencies2QueryHandler(CurrenciesDb db) 
  => _db = db;

  public async Task<List<Currency>> HandleAsync(
    GetCurrenciesQuery request
  , CancellationToken cancellationToken = default)
    => await _db.Currencies.AsNoTracking().ToListAsync(cancellationToken);
}

/// <summary>
/// Get all currencies query handler using DbContextFactory
/// </summary>
internal sealed class GetAllCurrencies3QueryHandler
: IQueryHandler<GetCurrenciesQuery, List<Currency>>
{
  private readonly IDbContextFactory<CurrenciesDb> _dbFactory;

  public GetAllCurrencies3QueryHandler(IDbContextFactory<CurrenciesDb> dbFactory) 
  => _dbFactory = dbFactory;

  public async Task<List<Currency>> HandleAsync(
    GetCurrenciesQuery request
  , CancellationToken cancellationToken = default)
  {
    // When using IDbContextFactory, it is important to manage the lifetime of the DbContext instances
    // properly. The instances created by the factory are not managed by the application's service provider
    // and must be disposed of by the application.
    await using CurrenciesDb db =
      await _dbFactory.CreateDbContextAsync(cancellationToken);

    return await db.Currencies.AsNoTracking().ToListAsync(cancellationToken);
  }
}
