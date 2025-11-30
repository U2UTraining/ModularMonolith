using ModularMonolith.APIs.BoundedContexts.BoardGames.DI;
using ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;
using ModularMonolith.APIs.BoundedContexts.Common.DI;
using ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;
using ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;
using ModularMonolith.ServiceDefaults;

namespace ModularMonolith.APIs;

public partial class Program
{
  private static void Main(string[] args)
  {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

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

    RouteGroupBuilder x = app.MapGroup("/currencies")
       .WithTags("Currencies")
       .WithCurrencyEndpoints()
       ;

    RouteGroupBuilder shoppingEndpoints =
      app.MapGroup("shopping")
         .WithTags("Shopping")
         .WithShoppingBasketEndpoints()
         ;

    app.AddGamesEndpoints();
    app.AddPublishersEndpoints();

    app.Run();
  }
}