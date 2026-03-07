using Microsoft.EntityFrameworkCore;
using ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;
using ModularMonolith.MigrationService;
using ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Testcontainers.MsSql;

namespace BoardGames.Tests;

public class GetGamesShould : IAsyncDisposable
{
  private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();

  [Before(Test)]
  public async Task Setup()
  {
    await _sqlContainer.StartAsync();
  }

  [After(Test)]
  public async Task Teardown()
  {
    await _sqlContainer.StopAsync();
  }

  public async ValueTask DisposeAsync()
  {
    await _sqlContainer.DisposeAsync();
  }

  [Test]
  public async Task ReturnAllGames()
  {
    DbContextOptions<BoardGamesDb> options =
      new DbContextOptionsBuilder<BoardGamesDb>()
      .UseSqlServer(_sqlContainer.GetConnectionString())
      .ConfigureWarnings(w 
      => w.Ignore(RelationalEventId.PendingModelChangesWarning))
      .Options;
    BoardGamesDb db = new BoardGamesDb(options);
    await db.Database.EnsureCreatedAsync();
    await Worker.SeedGamesAsync(db, CancellationToken.None);

    List<BoardGame> games = db.BoardGames.ToList();

    Assert.Equals(games.Count, 3);
  }
}
