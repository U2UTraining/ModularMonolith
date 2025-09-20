﻿using Microsoft.AspNetCore.Mvc;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public static class CurrencyEndpoints
{
  public static void AddCurrencyEndpoints(this WebApplication app)
  {
    RouteGroupBuilder currencies = app.MapGroup("/currencies")
      .WithTags("Currencies");

    _ = currencies.MapGet("/", 
      async (
        [FromServices] CurrenciesDb db
      , CancellationToken cancellationToken) =>
    {
      List<CurrencyDTO> allCurrencies = await db.Currencies
        .AsNoTracking()
        .Select(c => new CurrencyDTO(c.Id.ToString(), c.ValueInEuro))
        .ToListAsync(cancellationToken);
      return TypedResults.Ok(allCurrencies);
    })
    .WithName("GetAllCurrencies")
    .Produces<List<Currency>>(StatusCodes.Status200OK);

    currencies.MapPut("/", 
      async Task<Results<Ok<CurrencyDTO>, BadRequest<string>>>(
        [FromBody] CurrencyDTO dto
      , [FromServices] ICommandSender commandSender
      , CancellationToken cancellationToken) =>
    {
      if (Enum.TryParse<CurrencyName>(dto.CurrencyName, out CurrencyName currencyName) == false)
      {
        return TypedResults.BadRequest(error: $"Currency '{dto.CurrencyName}' is not valid.");
      }

      await commandSender.ExecuteAsync(
        new UpdateCurrencyValueInEuroCommand(currencyName, dto.ValueInEuro));
      return TypedResults.Ok(dto);
    })
    .WithName("UpdateCurrencyValue")
    .Produces<CurrencyDTO>(StatusCodes.Status200OK);
  }
}
