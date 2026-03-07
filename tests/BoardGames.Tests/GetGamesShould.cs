using Microsoft.EntityFrameworkCore;
using ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;
using ModularMonolith.MigrationService;
using ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BoardGames.Tests;

public class GetGamesShould
{
  [Test]
  public async Task ReturnAllGames()
  {
    DbContextOptions<BoardGamesDb> options =
      new DbContextOptionsBuilder<BoardGamesDb>()
      .UseSqlite($"Data Source={Guid.NewGuid()}.db")
      .ConfigureWarnings(w 
      => w.Ignore(RelationalEventId.PendingModelChangesWarning))
      .Options;
    BoardGamesDb db = new BoardGamesDb(options);
    //await db.Database.EnsureCreatedAsync();
    await db.Database.EnsureCreatedAsync();
    await Worker.SeedGamesAsync(db, CancellationToken.None);

    List<BoardGame> games = db.BoardGames.ToList();
  }
}
