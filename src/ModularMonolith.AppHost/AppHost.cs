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
      // Create a parameter resource for the SQL admin password and set its value.
      IResourceBuilder<ParameterResource> sqlPassword =
        builder.AddParameter(name: "sql-admin-password")
        ;

      _ = sql.RunAsContainer(sql =>
        sql.WithDataVolume("modularmonolith")
           .WithLifetime(ContainerLifetime.Persistent)
           // Pass the parameter resource (not a raw string) to WithPassword.
           .WithPassword(sqlPassword)
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

    IResourceBuilder<ProjectResource> ui =
      builder.AddProject<Projects.ModularMonolith_BlazorApp>("modular-monolith-ui")
        .WithReference(apis)
        .WaitFor(apis)
        ;

    if (builder.Environment.IsDevelopment())
    {
      IResourceBuilder<Smtp4devResource> smtp4dev = 
        builder.AddSmtp4dev(name: "smtp4dev", httpPort: 5000, smtpPort: 5001);
      _ = apis.WithReference(smtp4dev);
    }

    builder.Build().Run();
  }
}