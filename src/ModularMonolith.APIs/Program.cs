using ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;
using ModularMonolith.ServiceDefaults;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add support for bounded contexts
builder
  .AddCommon()
  .AddCurrencies()
//.AddBoardGames(
//  builder.Configuration.GetConnectionString("GamesDb")!)
//.AddShopping(builder.Configuration.GetConnectionString("ShoppingDb")!)
;

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.AddCurrencyEndpoints();

app.Run();
