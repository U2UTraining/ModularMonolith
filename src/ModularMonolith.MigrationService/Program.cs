using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ModularMonolith.MigrationService;
using ModularMonolith.ServiceDefaults;
using ModularMonolith.APIs.BoundedContexts.Currencies.Infra;
using ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;

internal class Program
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