
namespace ModularMonolith.MigrationService;

public partial class Worker
{
  private static async Task EnsureShoppingDatabaseAsync(
    ShoppingDb dbContext
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

  private static async Task RunShoppingMigrationAsync(
    ShoppingDb dbContext
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
}
