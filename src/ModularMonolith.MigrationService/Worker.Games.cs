using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.MigrationService;

public partial class Worker
{
  private static async Task EnsureGamesDatabaseAsync(
    GamesDb dbContext
  , CancellationToken cancellationToken)
  {
    IRelationalDatabaseCreator dbCreator =
      dbContext.GetService<IRelationalDatabaseCreator>();
    IExecutionStrategy strategy =
      dbContext.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
      // Create the database if it does not exist.
      // Do this first so there is then a database to start a transaction against.
      if (!await dbCreator.ExistsAsync(cancellationToken))
      {
        await dbCreator.CreateAsync(cancellationToken);
      }
    });
  }

  private static async Task RunGamesMigrationAsync(
    GamesDb dbContext
  , CancellationToken cancellationToken)
  {
    IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
      // Run migration in a transaction to avoid partial migration if it fails.
      await using var transaction =
        await dbContext.Database.BeginTransactionAsync(cancellationToken);
      await dbContext.Database.MigrateAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);
    });
  }

  private static async Task SeedGamesAsync(GamesDb dbContext, CancellationToken cancellationToken)
  {
    if (await dbContext.Publishers.AnyAsync(cancellationToken))
    {
      return;
    }
    Publisher _999Games = new(
      id: default
    , name: new PublisherName("999 Games")
    );
    BoardGame qwirkle = _999Games.CreateGame(
      new BoardGameName("Qwirkle")
    , new Money(29.95M, CurrencyName.EUR));
    qwirkle.SetImage(new Uri("https://u2ublogimages.blob.core.windows.net/cleanarchitecture/GamesStore_Qwirkle.png"));
    _999Games.AddContact(
      firstName: new NonEmptyString("Bram")
    , lastName: new NonEmptyString("De Bakker")
    , email: new EmailAddress("bram@u2u.be")
      );
    _999Games.AddContact(
      firstName: new NonEmptyString("Nico")
    , lastName: new NonEmptyString("De Bakker")
    , email: new EmailAddress("nico@u2u.be")
    );
    Publisher _Goliath = new(
      id: default
    , name: new PublisherName("Goliath")
    );
    BoardGame rummikub = _Goliath.CreateGame(
      new BoardGameName("Rummikub")
    , new Money(28.95M, CurrencyName.EUR));
    rummikub.SetImage(new Uri("https://u2ublogimages.blob.core.windows.net/cleanarchitecture/GamesStore_Rummikub.jpg"));
    _Goliath.AddContact(
      firstName: new NonEmptyString("Peter")
    , lastName: new NonEmptyString("Himschoot")
    , email: new EmailAddress("peter@u2u.be")
      );
    _Goliath.AddContact(
      firstName: new NonEmptyString("Maarten")
    , lastName: new NonEmptyString("De Bakker")
    , email: new EmailAddress("maarten@u2u.be")
    );
    Publisher _DaysOfWonder = new(
      id: default
    , name: new PublisherName("Days of Wonder")
    );
    BoardGame catan = _DaysOfWonder.CreateGame(
      new BoardGameName("Catan")
    , new Money(34.95M, CurrencyName.EUR));
    catan.SetImage(new Uri("https://u2ublogimages.blob.core.windows.net/cleanarchitecture/GamesStore_SettlersOfCatan.jpg"));

    await dbContext.Publishers.AddRangeAsync([
      _999Games, _Goliath, _DaysOfWonder
    ], cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);
  }
}
