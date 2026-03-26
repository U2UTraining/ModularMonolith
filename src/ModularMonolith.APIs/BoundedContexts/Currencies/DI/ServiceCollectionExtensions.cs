using ModularMonolith.APIs.EFCore.OutboxPattern;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.DI;

public static class ServiceCollectionExtensions
{
  public static IHostApplicationBuilder AddCurrencies(
    this IHostApplicationBuilder builder)
  {
    builder.Services
      .AddCurrencyServices()
      ;
    //.AddCurrenciesCore()
    //.AddCurrenciesQueries()
    //.AddCurrenciesCommands();

    builder.Services
      .AddKeyedSingleton<IOutboxSignal, OutboxSignal>(nameof(CurrenciesDb))
      .AddHostedService<OutboxHostedService<CurrenciesDb>>()
      ;

    builder.AddSqlServerDbContext<CurrenciesDb>(CurrenciesDb.DatabaseName
    , sqlServerOptions =>
    {
    }
    , optionsBuilder =>
    {
      optionsBuilder.AddInterceptors(
          new SoftDeleteInterceptor(),
          new AuditabilityInterceptor()
        );
      optionsBuilder.EnableDetailedErrors(true);
#if DEBUG
      optionsBuilder.EnableSensitiveDataLogging(true);
#endif
    });
    //_ = builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

    builder.Services.AddDbContextFactory<CurrenciesDb>();

    return builder;
  }
}
