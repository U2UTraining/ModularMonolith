using ModularMonolith.MigrationService;

namespace ModularMonolith.APIs.Tests.BoundedContexts.BoardGames;

public sealed class BoardGamesDbFactory
{
  public async Task<BoardGamesDb> CreateAsync(string connectionString)
  {
    DbContextOptions<BoardGamesDb> options =
    new DbContextOptionsBuilder<BoardGamesDb>()
    .UseSqlServer(connectionString)
    .ConfigureWarnings(w
      => w.Ignore(RelationalEventId.PendingModelChangesWarning))
    .Options;
    BoardGamesDb db = new(options);
    var unused = await db.Database.EnsureCreatedAsync();
    await Worker.SeedGamesAsync(db, CancellationToken.None);
    return db;
  }
}
