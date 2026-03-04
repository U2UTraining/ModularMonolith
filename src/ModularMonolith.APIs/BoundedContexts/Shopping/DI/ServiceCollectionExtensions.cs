namespace ModularMonolith.APIs.BoundedContexts.Shopping.DI;

public static class ServiceCollectionExtensions
{
  public static IHostApplicationBuilder AddShopping(
    this IHostApplicationBuilder builder)
  {
    builder.Services
      .AddShoppingServices()
    //.AddShoppingCommands()
    //.AddShoppingQueries();
    //.AddShoppingInfra(ShoppingDb.DatabaseName)
    ;
    builder.AddSqlServerDbContext<ShoppingDb>(ShoppingDb.DatabaseName,
      sqlServerOptions => {
      },
      optionsBuilder =>
      {
        optionsBuilder.AddInterceptors(
          new SoftDeleteInterceptor(),
          new AuditabilityInterceptor()
        );
        optionsBuilder.EnableDetailedErrors(true);
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging(true);
#endif
      }
    );
    //builder.Services.AddScoped<IShoppingRepository, ShoppingRepository>() ;

    builder.Services.AddDbContextFactory<ShoppingDb>();

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
//        sqlServerOptions.MigrationsAuditabilityTable(
//          tableName: AuditabilityRepository.DefaultTableName
//        , schema: ShoppingDb.SchemaName);
//      })
//      .AddInterceptors(
//        serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
//        serviceProvider.GetRequiredService<AuditabilityInterceptor>()
//      );
//      optionsBuilder.EnableDetailedErrors(true);
//#if DEBUG
//      optionsBuilder.EnableSensitiveDataLogging(true);
//#endif
//    });
//    ;
//    return services;
//  }

  //public static IServiceCollection AddShoppingQueries(
  //  this IServiceCollection services)
  //=> services
  //  .AddScoped<IQueryHandler<ShoppingBasketWithIdQuery, ShoppingBasketDto?>, ShoppingBasketWithIdQueryHandler>()
  //;

  //public static IServiceCollection AddShoppingCommands(
  //  this IServiceCollection services)
  //=> services
  //;
}
