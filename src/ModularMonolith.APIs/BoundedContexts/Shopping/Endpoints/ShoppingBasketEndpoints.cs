using System.Threading.Tasks;

using ModularMonolith.BoundedContexts.Common.Results;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

public static class ShoppingBasketEndpoints
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder WithShoppingBasketEndpoints()
    {
      //// Query DBContext directly
      //group.MapGet("/{id:int}", async (
      //  [FromRoute] int id
      //, [FromServices] ShoppingDb db
      //, CancellationToken cancellationToken) =>
      //{
      //  //List<ShoppingBasketDTO> allCurrencies = await db.
      //  //    .AsNoTracking()
      //  //    .Select(c => new ShoppingBasketDTO(c.Id.ToString(), c.ValueInEuro))
      //  //    .ToListAsync(cancellationToken);
      //  return TypedResults.Ok();
      //})
      //.WithName("GetAllCurrencies")
      //.Produces<List<Currency>>(StatusCodes.Status200OK);

      group.MapPost("/", async Task<Results<Ok<int>, NotFound>> (
        [FromServices] ShoppingDb db
      , CancellationToken cancellationToken
      ) =>
      {
        ShoppingBasket newShoppingBasket = new(0);
        db.Baskets.Add(newShoppingBasket);
        await db.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok(newShoppingBasket.Id.Key);
      })
      .WithName("CreateShoppingBasket");

      group.MapPut("/",
        async Task<Results<Ok, NotFound>> (
          [FromBody] AddBoardGameToShoppingBasketDTO dto
        , [FromServices] ShoppingDb db
        , [FromServices] IIntegrationEventPublisher eventPublisher
        , CancellationToken cancellationToken) =>
        {
          ShoppingBasket? sb =
            await db.Baskets
              .SingleOrDefaultAsync(b => b.Id == dto.ShoppingBasketId, cancellationToken);
          if (sb is not null)
          {
            sb.AddGame(dto.BoardGameId, new Money(dto.PriceInEuro));
            await db.SaveChangesAsync(cancellationToken);
            BoardGameSelectedForShoppingBasketIntegrationEvent e =
                  new(dto.BoardGameId, dto.ShoppingBasketId, dto.PriceInEuro);
            await eventPublisher.PublishIntegrationEventAsync(e);
            return TypedResults.Ok();
          }
          return TypedResults.NotFound();
        }
      )
      .WithName("AddBoardGameToShoppingBasket")
      ;
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
