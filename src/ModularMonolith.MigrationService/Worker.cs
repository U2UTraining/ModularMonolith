using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using U2U.ModularMonolith.BoundedContexts.Currencies.Entities;
using U2U.ModularMonolith.BoundedContexts.Currencies.Infra;
using U2U.ModularMonolith.BoundedContexts.Currencies.ValueObjects;

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

  private static async Task SeedCurrenciesAsync(CurrenciesDb dbContext, CancellationToken cancellationToken)
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
}

