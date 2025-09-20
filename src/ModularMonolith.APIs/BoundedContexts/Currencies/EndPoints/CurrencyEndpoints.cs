using Microsoft.AspNetCore.Mvc;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public static class CurrencyEndpoints
{
  public static void AddCurrencyEndpoints(this WebApplication app)
  {
    RouteGroupBuilder currencies = app.MapGroup("/currencies")
      .WithTags("Currencies");

    _ = currencies.MapGet("/", async ([FromServices] CurrenciesDb db) =>
    {
      List<Currency> allCurrencies = await db.Currencies
        .AsNoTracking()
        .ToListAsync();
      return TypedResults.Ok(allCurrencies);
    })
    .WithName("GetAllCurrencies")
    .Produces<List<Currency>>(StatusCodes.Status200OK);
  }
}
