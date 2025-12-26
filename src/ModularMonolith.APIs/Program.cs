using ModularMonolith.APIs.BoundedContexts.BoardGames.DI;
using ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;
using ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;
using ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;
using ModularMonolith.APIs.BoundedContexts.UI.EndPoints;
using ModularMonolith.ServiceDefaults;

namespace ModularMonolith.APIs;

public static partial class Program
{
  private static void Main(string[] args)
  {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    builder.Services.AddValidation(options =>
    {
      //options.
    });

    // Add support for bounded contexts
    builder
      .AddCommon()
      .AddEmailServices()
      .AddCurrencies()
      .AddBoardGames()
      .AddShopping()
      ;

    WebApplication app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app
      .MapGroup("/currencies")
      .
    GetWithCurrencyEndpoints()
      .WithTags("Currencies")
      ;

    app
      .MapGroup("/games")
      .WithBoardGameEndpoints()
      .WithTags("Games")
      ;

    app
      .MapGroup("/publishers")
      .WithPublisherEndpoints()
      .WithTags("Publishers")
      ;

    app
      .MapGroup("shopping")
      .WithShoppingBasketEndpoints()
      .WithTags("Shopping")
      ;

    app.MapGroup("ui")
       .WithUIEndpoints()
       .WithTags("UI")
       ;

    app.Run();
  }
}