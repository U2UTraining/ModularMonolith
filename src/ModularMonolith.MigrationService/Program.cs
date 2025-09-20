using Microsoft.Extensions.Hosting;

using ModularMonolith.ServiceDefaults;

internal class Program
{
  private static void Main(string[] args)
  {
    var builder = Host.CreateApplicationBuilder(args);

    builder.AddServiceDefaults();
    //builder.Services.AddHostedService<Worker>();
    //builder.Services.AddOpenTelemetry()
    //    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

    //builder.AddSqlServerDbContext<ApplicationDbContext>("users-db");
    //builder.AddSqlServerDbContext<PostsContext>("posts-db");
    var host = builder.Build();
    host.Run();
  }
}