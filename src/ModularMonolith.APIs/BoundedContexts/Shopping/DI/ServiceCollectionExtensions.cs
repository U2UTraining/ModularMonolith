using ModularMonolith.APIs.BoundedContexts.Shopping.QueryHandlers;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.DI;

public static class ServiceCollectionExtensions
{
  public static IHostApplicationBuilder AddShopping(
    this IHostApplicationBuilder builder)
  {
    builder.Services
    .AddShoppingCommands()
    .AddShoppingQueries();
    //.AddShoppingInfra(ShoppingDb.DatabaseName);
    builder.AddSqlServerDbContext<ShoppingDb>(ShoppingDb.DatabaseName,
      sqlServerOptions => {
      },
      optionsBuilder =>
      {
        optionsBuilder.AddInterceptors(
          new SoftDeleteInterceptor(),
          new HistoryInterceptor()
        );
        optionsBuilder.EnableDetailedErrors(true);
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging(true);
#endif
      }
    );
    //builder.Services.AddScoped<IShoppingRepository, ShoppingRepository>() ;
    return builder;
  }

//  public static IServiceCollection AddShoppingInfra(
//    this IServiceCollection services
//  , string connectionString)
//  {
//    services
//    .AddDbContext<ShoppingDb>((serviceProvider, optionsBuilder) =>
//    {
//      optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
//      {
//        sqlServerOptions.EnableRetryOnFailure(3);
//        sqlServerOptions.UseCompatibilityLevel(160);
//        sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
//        // Migrations
//        sqlServerOptions.MigrationsHistoryTable(
//          tableName: HistoryRepository.DefaultTableName
//        , schema: ShoppingDb.SchemaName);
//      })
//      .AddInterceptors(
//        serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
//        serviceProvider.GetRequiredService<HistoryInterceptor>()
//      );
//      optionsBuilder.EnableDetailedErrors(true);
//#if DEBUG
//      optionsBuilder.EnableSensitiveDataLogging(true);
//#endif
//    });
//    ;
//    return services;
//  }

  public static IServiceCollection AddShoppingQueries(
    this IServiceCollection services)
  => services
    .AddScoped<IQueryHandler<ShoppingBasketWithIdQuery, ShoppingBasketDTO?>, ShoppingBasketWithIdQueryHandler>()
  ;

  public static IServiceCollection AddShoppingCommands(
    this IServiceCollection services)
  => services
  ;
}
