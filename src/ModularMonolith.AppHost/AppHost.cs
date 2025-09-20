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

    //var usersDb = sql.AddDatabase("mm-currency-db");

    IResourceBuilder<ProjectResource> apis = 
      builder.AddProject<Projects.ModularMonolith_APIs>("modular-monolith-apis")
    //.WithReference(postsDb)
    //.WaitForCompletion(migrations)
    ;

    if (builder.Environment.IsDevelopment())
    {
      var smtp4dev = builder.AddSmtp4dev("smtp4dev");
      //web.WithReference(smtp4dev);
    }

    builder.Build().Run();
  }
}