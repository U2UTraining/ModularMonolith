namespace ModularMonolith.APIs.BoundedContexts.Shopping.DI;

public static class ServiceCollectionExtensions
{
  public static IHostApplicationBuilder AddShopping(
    this IHostApplicationBuilder builder)
  {
    builder.Services
    .AddShoppingCommands()
    .AddShoppingQueries()
    .AddShoppingInfra(ShoppingDb.DatabaseName)
    ;
    return builder;
  }

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
