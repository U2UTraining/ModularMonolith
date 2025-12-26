using ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

public static class ShoppingBasketEndpoints
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder WithShoppingBasketEndpoints()
    {
      //// Query DBContext directly
      group.MapGet("/{id:int}", async Task<Results<Ok<ShoppingBasketDto>, NotFound>> (
        [FromRoute] int id
      , [FromServices] IQuerySender querySender
      , CancellationToken cancellationToken) =>
      {
        ShoppingBasketDto? dto = await querySender.AskAsync(
          new ShoppingBasketWithIdQuery(id, includeGames: true), cancellationToken);
        if (dto is null)
        {

          return TypedResults.NotFound();
        }
        else
        {
        
        return TypedResults.Ok(dto);
        }

          //ShoppingBasket? basket = await db.GetShoppingBasketAsync(id, cancellationToken);
          //ShoppingBasketDTO dto = new ShoppingBasketDTO(
          //  ShoppingBasketId: basket!.Id.Key
          //, Games: basket.Items.Select(it => new GameDTO(
          //    Id: it.BoardGameId.Key,
          //    GameName: "",
          //    Price: it.Price
          //    )))

          //ShoppingBasketDTO dto =
          //  await db.Baskets.Where(sb=>sb.Id == id)
          //  .Select( sb => new ShoppingBasketDTO(
          //    sb.Id
          //  , sb.Games.Select())
          //List<ShoppingBasketDTO> allCurrencies = await db.Baskets
          //    .AsNoTracking()
          //    .Where( b => b.Id == id)
          //    .Select(c => new ShoppingBasketDTO(
          //      ShoppingBasketId: c.Id.Key,
          //      Games: c.Items
          //        .Select(gi => new GameDTO(
          //          Id: gi.BoardGameId.Key,
          //          GameName: gi.BoardGame!.Name,
          //          PriceInEuro: gi.Price.Value
          //        ))
          //        .ToList()


          //      ))
          //    .SingleAsync(cancellationToken);
          //return TypedResults.Ok();
      })
      .WithName("GetShoppingBasketWithId")
      .Produces<List<Currency>>(StatusCodes.Status200OK);

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
          [FromBody] AddBoardGameToShoppingBasketDto dto
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
            BoardGameSelectedForShoppingBasketIntegrationEvent e = new(
              ShoppingBasketId: dto.ShoppingBasketId
            , BoardGameId: dto.BoardGameId
            , PriceInEuro: dto.PriceInEuro
            );
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
