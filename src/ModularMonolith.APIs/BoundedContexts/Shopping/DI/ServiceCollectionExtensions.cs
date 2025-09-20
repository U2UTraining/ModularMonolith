using ModularMonolithBoundedContexts.Shopping.Infra;
using ModularMonolithBoundedContexts.Shopping.Repositories;

namespace ModularMonolithBoundedContexts.Shopping.DI;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddShopping(
    this IServiceCollection services
  , string connectionString)
  => services
    .AddShoppingQueries()
    .AddShoppingCommands()
    .AddShoppingInfra(connectionString)
  ;

  public static IServiceCollection AddShoppingInfra(
    this IServiceCollection services
  , string connectionString)
  {
    services
    .AddDbContext<ShoppingDb>((serviceProvider, optionsBuilder) =>
    {
      optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
      {
        sqlServerOptions.EnableRetryOnFailure(3);
        sqlServerOptions.UseCompatibilityLevel(160);
        sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
        // Migrations
        sqlServerOptions.MigrationsHistoryTable(
          tableName: HistoryRepository.DefaultTableName
        , schema: ShoppingDb.SchemaName);
      })
      .AddInterceptors(
        serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
        serviceProvider.GetRequiredService<HistoryInterceptor>()
      );
    });
    services.AddScoped<IShoppingRepository, ShoppingRepository>()
    ;
    return services;
  }

  public static IServiceCollection AddShoppingQueries(
  this IServiceCollection services)
=> services
;

  public static IServiceCollection AddShoppingCommands(
    this IServiceCollection services)
  => services
  ;


}
