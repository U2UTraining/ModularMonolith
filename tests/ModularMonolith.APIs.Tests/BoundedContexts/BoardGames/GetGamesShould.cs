using ModularMonolith.MigrationService;

namespace ModularMonolith.APIs.Tests.BoundedContexts.BoardGames;

public class GetGamesShould : IAsyncDisposable
{
  private readonly MsSqlContainer _sqlContainer =
    new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
    .Build();

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
    BoardGamesDb db = 
      await new BoardGamesDbFactory()
      .CreateAsync(_sqlContainer.GetConnectionString());
    List<BoardGame> games = db.BoardGames.ToList();
    await Assert.That(games.Count).IsEqualTo(3);
  }
}
