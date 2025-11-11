namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

public static class ShoppingBasketEndpoints
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder WithShoppingBasketEndpoints()
    {
      // Query DBContext directly
      group.MapGet("/{id:int}", async (
        [FromRoute] int id
      , [FromServices] ShoppingDb db
      , CancellationToken cancellationToken) =>
      {
        //List<ShoppingBasketDTO> allCurrencies = await db.
        //    .AsNoTracking()
        //    .Select(c => new ShoppingBasketDTO(c.Id.ToString(), c.ValueInEuro))
        //    .ToListAsync(cancellationToken);
        return TypedResults.Ok();
      })
      .WithName("GetAllCurrencies")
      .Produces<List<Currency>>(StatusCodes.Status200OK);

      //group.MapPut("/",
      //  async Task<Results<Ok<CurrencyDTO>, BadRequest<string>>> (
      //    [FromBody] CurrencyDTO dto
      //  , [FromServices] ICommandSender commandSender
      //  , CancellationToken cancellationToken) =>
      //  {
      //    if (Enum.TryParse(dto.CurrencyName, out CurrencyName currencyName) == false)
      //    {
      //      return TypedResults.BadRequest(error: $"Currency '{dto.CurrencyName}' is not valid.");
      //    }
      //    _ = await commandSender.ExecuteAsync(
      //      new UpdateCurrencyValueInEuroCommand(currencyName, dto.ValueInEuro));
      //    return TypedResults.Ok(dto);
      //  })
      //.WithName("UpdateCurrencyValue")
      //.Produces<CurrencyDTO>(StatusCodes.Status200OK);
      return group;
    }
  }
}
