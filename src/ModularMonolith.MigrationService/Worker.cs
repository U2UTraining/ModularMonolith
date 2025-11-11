
using System.Diagnostics;

using ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;

namespace ModularMonolith.MigrationService;

public partial class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
  public const string ActivitySourceName = "Migrations";
  private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    using Activity? activity = 
      s_activitySource.StartActivity("Migrating databases", ActivityKind.Client);

    try
    {
      using IServiceScope scope = serviceProvider.CreateScope();
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

      ShoppingDb shoppingContext =
        scope.ServiceProvider.GetRequiredService<ShoppingDb>();

      await EnsureShoppingDatabaseAsync(shoppingContext, cancellationToken);
      await RunShoppingMigrationAsync(shoppingContext, cancellationToken);
    }
    catch (Exception ex)
    {
      activity?.AddException(ex);
      throw;
    }

    hostApplicationLifetime.StopApplication();
  }
}

