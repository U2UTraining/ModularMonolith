using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore.InMemory; 


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
      .UseSqlite(Guid.NewGuid().ToString())
      .Options;
    GamesDb db = new GamesDb(options);
    await db.Database.MigrateAsync();
    await Worker.SeedGamesAsync(db, CancellationToken.None);

    List<BoardGame> games = db.Games.ToList();
  }
}
