using TUnit.Core.Interfaces;

namespace ModularMonolith.APIs.Tests.EFCore.OutboxPattern;

/// <summary>
/// Shared fixture that starts a SQL Server container once for all tests in the collection.
/// Implements IAsyncInitializer so TUnit calls InitializeAsync before the first test runs,
/// and IAsyncDisposable so the container is cleaned up afterwards.
/// </summary>
public class SqlServerContainerFixture : IAsyncInitializer, IAsyncDisposable
{
  public SqlServerContainerFixture()
  {
    _sqlContainer =
      new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
      .Build();
  }

  private readonly MsSqlContainer _sqlContainer;
  public string ConnectionString
    => _sqlContainer.GetConnectionString();

  public async Task InitializeAsync()
  {
    await _sqlContainer.StartAsync();
    DbContextOptions<CurrenciesDb> options =
      new DbContextOptionsBuilder<CurrenciesDb>()
      .UseSqlServer(_sqlContainer.GetConnectionString())
      .ConfigureWarnings(w
        => w.Ignore(RelationalEventId.PendingModelChangesWarning))
      .Options;
    using CurrenciesDb db = new CurrenciesDb(options);
    await db.Database.EnsureCreatedAsync();
  }

  public async ValueTask DisposeAsync()
    => await _sqlContainer.DisposeAsync();

  public CurrenciesDb CreateCurrenciesDb()
  {
    DbContextOptions<CurrenciesDb> options =
    new DbContextOptionsBuilder<CurrenciesDb>()
    .UseSqlServer(_sqlContainer.GetConnectionString())
    .ConfigureWarnings(w
      => w.Ignore(RelationalEventId.PendingModelChangesWarning))
    .Options;
    return new CurrenciesDb(options);
  }
}
