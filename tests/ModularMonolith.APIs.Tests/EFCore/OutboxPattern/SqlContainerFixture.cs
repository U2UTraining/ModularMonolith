using Xunit;

namespace ModularMonolith.APIs.Tests.EFCore.OutboxPattern;

public class SqlServerContainerFixture 
: IAsyncLifetime
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

  public async ValueTask InitializeAsync()
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

[CollectionDefinition("SqlServer")]
public class SqlServerCollection : ICollectionFixture<SqlServerContainerFixture>
{
}

