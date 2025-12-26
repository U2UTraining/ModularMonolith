namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public static class CurrencyEndpoints
{
  extension(RouteGroupBuilder group)
  {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public RouteGroupBuilder GetWithCurrencyEndpoints()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
      group.MapGet("/", CurrencyEndpoints.GetAllCurrencies)
        .WithName(nameof(GetAllCurrencies))
        .Produces<List<CurrencyDto>>(StatusCodes.Status200OK);

      group.MapPut("/", CurrencyEndpoints.UpdateCurrencyValue)
        .WithName(nameof(UpdateCurrencyValue))
        .Produces<CurrencyDto>(StatusCodes.Status200OK);

      return group;
    }

    public static async Task<Results<Ok<List<CurrencyDto>>, BadRequest>> GetAllCurrencies(
      [FromServices] IQuerySender querySender
    , CancellationToken cancellationToken = default)
    {
      List<Currency> currencies =
         await querySender.AskAsync(GetCurrenciesQuery.All, cancellationToken);
      List<CurrencyDto> allCurrencies =
        currencies.Select(c => new CurrencyDto(c.Id.ToString(), c.ValueInEuro)).ToList();
      return TypedResults.Ok(allCurrencies);
    }

    public static async Task<Results<Ok<CurrencyDto>, BadRequest<string>>> UpdateCurrencyValue(
      [FromBody] CurrencyDto dto
    , [FromServices] ICommandSender commandSender
    , CancellationToken cancellationToken)
    {
      if (!Enum.TryParse(dto.CurrencyName, out CurrencyName currencyName))
      {
        return TypedResults.BadRequest(error: $"Currency '{dto.CurrencyName}' is not valid.");
      }
      _ = await commandSender.ExecuteAsync(
        new UpdateCurrencyValueInEuroCommand(currencyName, dto.ValueInEuro), cancellationToken);
      return TypedResults.Ok(dto);
    }
  }
}
