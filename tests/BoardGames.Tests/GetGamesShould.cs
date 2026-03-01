using Microsoft.EntityFrameworkCore;
using ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;
using ModularMonolith.MigrationService;
using ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;

namespace BoardGames.Tests;

public class GetGamesShould
{
  [Test]
  public async Task ReturnAllGames()
  {
    DbContextOptions<GamesDb> options =
      new DbContextOptionsBuilder<GamesDb>()
      .UseSqlite($"Data Source={Guid.NewGuid()}.db")
      .Options;
    GamesDb db = new GamesDb(options);
    //await db.Database.EnsureCreatedAsync();
    await db.Database.MigrateAsync();
    await Worker.SeedGamesAsync(db, CancellationToken.None);

    List<BoardGame> games = db.Games.ToList();
  }
}
