using ModularMonolith.MigrationService;

namespace ModularMonolith.APIs.Tests.BoundedContexts.Currencies;

public sealed class CurrencyDbFactory
{
  public async Task<CurrenciesDb> CreateAsync(string connectionString)
  {
    DbContextOptions<CurrenciesDb> options =
    new DbContextOptionsBuilder<CurrenciesDb>()
    .UseSqlServer(connectionString)
    .ConfigureWarnings(w
      => w.Ignore(RelationalEventId.PendingModelChangesWarning))
    .Options;
    CurrenciesDb db = new(options);
    bool unused = await db.Database.EnsureCreatedAsync();
    await Worker.SeedCurrenciesAsync(db, CancellationToken.None);
    return db;
  }
}
