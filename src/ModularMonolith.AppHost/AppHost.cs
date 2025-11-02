using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;
using ModularMonolith.Smtp4Dev.Hosting;

internal class Program
{
  private static void Main(string[] args)
  {
    IDistributedApplicationBuilder builder =
      DistributedApplication.CreateBuilder(args);

    IResourceBuilder<AzureSqlServerResource> sql =
      builder.AddAzureSqlServer("sql");
    if (builder.Environment.IsDevelopment())
    {
      _ = sql.RunAsContainer(sql =>
        sql.WithDataVolume("modularmonolith")
           .WithLifetime(ContainerLifetime.Persistent)
      );
    }

    IResourceBuilder<AzureSqlDatabaseResource> currencyDb = 
      sql.AddDatabase("mm-currency-db");
    IResourceBuilder<AzureSqlDatabaseResource> gamesDb =
      sql.AddDatabase("mm-games-db");

    IResourceBuilder<ProjectResource> migrations = 
      builder.AddProject<Projects.ModularMonolith_MigrationService>("migrationservice")
        .WithReference(currencyDb)
        .WaitFor(currencyDb)
        .WithReference(gamesDb)
        .WaitFor(gamesDb)
        ;

    IResourceBuilder<ProjectResource> apis = 
      builder.AddProject<Projects.ModularMonolith_APIs>("modular-monolith-apis")
        .WithReference(currencyDb)
        .WithReference(gamesDb)
        .WaitForCompletion(migrations)
        ;

    var ui = 
      builder.AddProject<Projects.ModularMonolith_BlazorApp>("modular-monolith-ui")
        .WithReference(apis)
        .WaitFor(apis)
        ;

    if (builder.Environment.IsDevelopment())
    {
      var smtp4dev = builder.AddSmtp4dev("smtp4dev");
      apis.WithReference(smtp4dev);
    }

    builder.Build().Run();
  }
}