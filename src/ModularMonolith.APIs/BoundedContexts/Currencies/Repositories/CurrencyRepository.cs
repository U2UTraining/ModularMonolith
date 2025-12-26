using ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.Repositories;

public static class CurrencyRepositoryExtensions
{
  extension(CurrenciesDb db)
  {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public async Task<List<CurrencyDto>> GetAllCurrenciesAsync(
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
      CancellationToken cancellationToken = default)
    {
      return await db.Currencies
          .AsNoTracking()
          .Select(c => new CurrencyDto(c.Id.ToString(), c.ValueInEuro))
          .ToListAsync(cancellationToken);
    }
  }
}
