using Microsoft.Extensions.DependencyInjection;

using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.DomainEvents;
using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.DI;

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

    builder.Services.AddDbContextFactory<CurrenciesDb>();

    return builder;
  }

  public static IServiceCollection AddCurrenciesQueries(
    this IServiceCollection services)
  => services
    //.AddScoped<
    //  IQueryHandler<GetCurrenciesQuery, IQueryable<Currency>>,
    //  GetAllCurrenciesQueryHandler>()
    //.AddScoped<
    //  IQueryHandler<GetCurrenciesQuery, IQueryable<Currency>>,
    //  GetAllCurrenciesQueryHandler2>()
    .AddScoped<
      IQueryHandler<GetCurrenciesQuery, List<Currency>>,
      GetAllCurrencies3QueryHandler>()
      ;

  public static IServiceCollection AddCurrenciesCommands(
  this IServiceCollection services)
  => services
    .AddScoped<
      ICommandHandler<UpdateCurrencyValueInEuroCommand, Currency>
    , UpdateCurrencyValueInEuroCommandHandler>()
    .AddSingleton<IValidator<UpdateCurrencyValueInEuroCommand>, UpdateCurrencyValueInEuroValidator>()
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
    .AddScoped<
      IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
    , CurrencyValueInEuroHasChangedEmailDomainEventHandler>()
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
