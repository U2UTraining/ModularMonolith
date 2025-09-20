using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ModularMonolith.BoundedContexts.BoardGames.Entities;
using ModularMonolith.BoundedContexts.BoardGames.Infra;
using ModularMonolith.BoundedContexts.BoardGames.ValueObjects;
using ModularMonolith.BoundedContexts.Common.ValueObjects;
using ModularMonolith.BoundedContexts.Currencies.Entities;
using ModularMonolith.BoundedContexts.Currencies.Infra;
using ModularMonolith.BoundedContexts.Currencies.ValueObjects;

namespace ModularMonolith.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
  public const string ActivitySourceName = "Migrations";
  private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    using var activity = s_activitySource.StartActivity("Migrating databases", ActivityKind.Client);

    try
    {
      using var scope = serviceProvider.CreateScope();
      CurrenciesDb currencyContext =
        scope.ServiceProvider.GetRequiredService<CurrenciesDb>();

      await EnsureCurrencyDatabaseAsync(currencyContext, cancellationToken);
      await RunCurrencyMigrationAsync(currencyContext, cancellationToken);
      await SeedCurrenciesAsync(currencyContext, cancellationToken);

      GamesDb gamesContext =
        scope.ServiceProvider.GetRequiredService<GamesDb>();

      await EnsureGamesDatabaseAsync(gamesContext, cancellationToken);
      await RunGamesMigrationAsync(gamesContext, cancellationToken);
      await SeedGamesAsync(gamesContext, cancellationToken);

      //var postsContext = scope.ServiceProvider.GetRequiredService<PostsContext>();

      //await EnsurePostsDatabaseAsync(postsContext, cancellationToken);
      //await RunPostsMigrationAsync(postsContext, cancellationToken);

      //await SeedUsersAndPostsAsync(dbContext, postsContext, cancellationToken);
    }
    catch (Exception ex)
    {
      activity?.AddException(ex);
      throw;
    }

    hostApplicationLifetime.StopApplication();
  }

  private static async Task EnsureCurrencyDatabaseAsync(
    CurrenciesDb dbContext
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

  private static async Task RunCurrencyMigrationAsync(
    CurrenciesDb dbContext
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

  private static async Task SeedCurrenciesAsync(
    CurrenciesDb dbContext
  , CancellationToken cancellationToken)
  {
    if (await dbContext.Currencies.AnyAsync(cancellationToken))
    {
      return;
    }
    List<Currency> currencies = new List<Currency>
    {
      new Currency(CurrencyName.EUR, 1.0M),
      new Currency(CurrencyName.USD, 0.85M),
      new Currency(CurrencyName.JPY, 0.0058M),
    };
    await dbContext.Currencies.AddRangeAsync(currencies, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);
  }

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

