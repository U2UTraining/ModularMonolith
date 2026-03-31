namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public static class CurrencyEndpoints
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder GetWithCurrencyEndpoints()
    {
      group.MapGet("/", async (GetAllCurrencies handler, CancellationToken cancellationToken)
        => await handler.ExecuteAsync(cancellationToken))
        .WithName(nameof(GetAllCurrencies))
        .WithSummary("Get all currencies")
        .WithDescription("Returns all supported currencies with their current value in EUR.")
        .Produces<List<CurrencyDto>>(StatusCodes.Status200OK);

      group.MapPut("/", async (UpdateCurrencyValue handler
        , CurrencyDto dto
        , CancellationToken cancellationToken)
        => await handler.ExecuteAsync(dto, cancellationToken))
        .WithName(nameof(UpdateCurrencyValue))
        .WithSummary("Update a currency value")
        .WithDescription("Updates the EUR exchange rate for the specified currency. The EUR rate itself cannot be modified.")
        .Produces<CurrencyDto>(StatusCodes.Status200OK)
        .Produces<string>(StatusCodes.Status400BadRequest);
 
      return group;
    }
  }
}
