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
    // Query Handler stuff...
    return result;
  }
}
