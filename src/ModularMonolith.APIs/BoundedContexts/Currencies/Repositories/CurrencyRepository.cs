using ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.Repositories;

public static class CurrencyRepositoryExtensions
{
  extension(CurrenciesDb db)
  {
    public async Task<List<CurrencyDTO>> GetAllCurrenciesAsync(
      CancellationToken cancellationToken = default)
    {
      return await db.Currencies
          .AsNoTracking()
          .Select(c => new CurrencyDTO(c.Id.ToString(), c.ValueInEuro))
          .ToListAsync(cancellationToken);
    }
  }
}
