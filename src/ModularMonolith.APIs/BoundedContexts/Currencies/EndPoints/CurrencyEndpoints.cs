namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public static class CurrencyEndpoints
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder GetWithCurrencyEndpoints()
    {
      group.MapGet("/", async (GetAllCurrencies handler)
        => await handler.ExecuteAsync())
        .WithName(nameof(GetAllCurrencies))
        .Produces<List<CurrencyDto>>(StatusCodes.Status200OK);

      group.MapPut("/", async (UpdateCurrencyValue handler
        , CurrencyDto dto
        , CancellationToken cancellationToken)
        => await handler.ExecuteAsync(dto, cancellationToken))
        .WithName(nameof(UpdateCurrencyValue))
        .Produces<CurrencyDto>(StatusCodes.Status200OK);
 
      return group;
    }
  }
}
