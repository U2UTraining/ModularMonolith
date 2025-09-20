//using ModularMonolith.BoundedContexts.Shopping.IntegrationEventHandlers;
using Microsoft.EntityFrameworkCore;
namespace ModularMonolith.BoundedContexts.Currencies.DI;

public static class ServiceCollectionExtensions
{
  public static IHostApplicationBuilder AddCurrencies(
    this IHostApplicationBuilder builder)
  {
    builder.Services
      .AddCurrenciesCore()
      .AddCurrenciesQueries()
      .AddCurrenciesCommands();
    builder.AddSqlServerDbContext<CurrenciesDb>(CurrenciesDb.DatabaseName
    , sqlServerOptions => {
    }
    , optionsBuilder => {
      optionsBuilder.AddInterceptors(
          new SoftDeleteInterceptor(),
          new HistoryInterceptor()
        );
      optionsBuilder.EnableDetailedErrors(true);
    #if DEBUG
      optionsBuilder.EnableSensitiveDataLogging(true);
    #endif
    });
    _ = builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

    return builder;
  }

  //public static IServiceCollection AddCurrencies(
  //  this IServiceCollection services
  //, string connectionString)
  //=> services
  //  .AddCurrenciesCore()
  //  .AddCurrenciesQueries()
  //  .AddCurrenciesCommands()
  //  //.AddCurrenciesInfra(connectionString)
  //  ;

  public static IServiceCollection AddCurrenciesQueries(
    this IServiceCollection services)
  => services
    .AddScoped<
      IQueryHandler<GetCurrenciesQuery, IQueryable<Currency>>,
      GetAllCurrenciesQueryHandler>()
      ;

  public static IServiceCollection AddCurrenciesCommands(
  this IServiceCollection services)
  => services
    .AddScoped<
      ICommandHandler<UpdateCurrencyValueInEuroCommand, Currency>
    , UpdateCurrencyValueInEuroCommandHandler>()
      ;

  public static IServiceCollection AddCurrenciesCore(
    this IServiceCollection services
  )
  => services
    .AddScoped<
      IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
    , CurrencyValueInEuroHasChangedLoggingDomainEventHandler>()
    .AddScoped<
      IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
    , CurrencyValueInEuroHasChangedDomainEventHandler>()
    // TEMP: MOve
    //.AddScoped<
    //  IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>
    //, CurrencyHasChangedIntegrationEventHandler>()
    ;

  public static IServiceCollection AddCurrenciesInfra(
    this IServiceCollection services
  , string connectionString
  )
  {


    _ = services.AddDbContext<CurrenciesDb>((serviceProvider, optionsBuilder) =>
    {
      optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
      {
        sqlServerOptions.EnableRetryOnFailure(3);
        sqlServerOptions.UseCompatibilityLevel(160);
        sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
        // Migrations
        sqlServerOptions.MigrationsHistoryTable(
          tableName: HistoryRepository.DefaultTableName
        , schema: CurrenciesDb.SchemaName);
      })
      .AddInterceptors(
        serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
        serviceProvider.GetRequiredService<HistoryInterceptor>()
      );
      optionsBuilder.EnableDetailedErrors(true);
#if DEBUG
      optionsBuilder.EnableSensitiveDataLogging(true);
#endif
    }
    //This is weirdly important!Using Singleton scoping
    //of the options allows Wolverine to significantly
    //optimize the runtime pipeline of the handlers that
    //use this DbContext type
    , optionsLifetime: ServiceLifetime.Singleton
    );
    _ = services.AddScoped<ICurrencyRepository, CurrencyRepository>();
    return services;
  }
}
