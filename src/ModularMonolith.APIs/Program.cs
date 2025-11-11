using ModularMonolith.APIs.BoundedContexts.BoardGames.DI;
using ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;
using ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;
using ModularMonolith.ServiceDefaults;

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

//app.AddCurrencyEndpoints();
app.AddGamesEndpoints();
app.AddPublishersEndpoints();

app.Run();
