using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ModularMonolith.MigrationService;
using ModularMonolith.ServiceDefaults;

using ModularMonolithBoundedContexts.Currencies.Infra;

internal class Program
{
  private static void Main(string[] args)
  {
    var builder = Host.CreateApplicationBuilder(args);

    builder.AddServiceDefaults();
    builder.Services.AddHostedService<Worker>();
    builder.Services.AddOpenTelemetry()
        .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

    //builder.AddSqlServerDbContext<ApplicationDbContext>("users-db");
    //builder.AddSqlServerDbContext<PostsContext>("posts-db");

    builder.AddSqlServerDbContext<CurrenciesDb>(CurrenciesDb.DatabaseName);

    var host = builder.Build();
    host.Run();
  }
}