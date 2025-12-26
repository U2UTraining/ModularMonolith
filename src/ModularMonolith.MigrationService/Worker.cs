
using System.Diagnostics;

using ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;

namespace ModularMonolith.MigrationService;

public partial class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
  public const string ActivitySourceName = "Migrations";
  private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    using Activity? activity = 
      s_activitySource.StartActivity("Migrating databases", ActivityKind.Client);

    try
    {
      using IServiceScope scope = serviceProvider.CreateScope();
      CurrenciesDb currencyContext =
        scope.ServiceProvider.GetRequiredService<CurrenciesDb>();

      await EnsureCurrencyDatabaseAsync(currencyContext, stoppingToken);
      await RunCurrencyMigrationAsync(currencyContext, stoppingToken);
      await SeedCurrenciesAsync(currencyContext, stoppingToken);

      GamesDb gamesContext =
        scope.ServiceProvider.GetRequiredService<GamesDb>();

      await EnsureGamesDatabaseAsync(gamesContext, stoppingToken);
      await RunGamesMigrationAsync(gamesContext, stoppingToken);
      await SeedGamesAsync(gamesContext, stoppingToken);

      ShoppingDb shoppingContext =
        scope.ServiceProvider.GetRequiredService<ShoppingDb>();

      await EnsureShoppingDatabaseAsync(shoppingContext, stoppingToken);
      await RunShoppingMigrationAsync(shoppingContext, stoppingToken);
    }
    catch (Exception ex)
    {
      activity?.AddException(ex);
      throw;
    }

    hostApplicationLifetime.StopApplication();
  }
}

