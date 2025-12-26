using ModularMonolith.ServiceDefaults;

namespace ModularMonolith.MigrationService;

internal static class Program
{
  private static void Main(string[] args)
  {
    var builder = Host.CreateApplicationBuilder(args);

    builder.AddServiceDefaults();
    builder.Services.AddHostedService<Worker>();
    builder.Services.AddOpenTelemetry()
        .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

    builder.AddSqlServerDbContext<CurrenciesDb>(CurrenciesDb.DatabaseName);
    builder.AddSqlServerDbContext<GamesDb>(GamesDb.DatabaseName);
    builder.AddSqlServerDbContext<ShoppingDb>(ShoppingDb.DatabaseName);

    var host = builder.Build();
    host.Run();
  }
}