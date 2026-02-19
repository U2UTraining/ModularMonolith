
namespace ModularMonolith.APIs.BoundedContexts.Currencies.Repositories;

public static class CurrencyRepositoryExtensions
{
  extension(CurrenciesDb db)
  {
    public async Task<List<CurrencyDto>> GetAllCurrenciesAsync(
      CancellationToken cancellationToken = default) 
    => await db.Currencies
               .AsNoTracking()
               .Select(c => new CurrencyDto(c.Id.ToString(), c.ValueInEuro))
               .ToListAsync(cancellationToken);
  }
}
