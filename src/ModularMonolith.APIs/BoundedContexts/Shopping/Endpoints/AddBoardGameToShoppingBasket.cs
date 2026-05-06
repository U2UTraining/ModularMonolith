namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShoppingServices")]
public sealed class AddBoardGameToShoppingBasket(
  ShoppingDb db
, IQuerySender querySender
, [FromKeyedServices(nameof(BoardGamesDb))] IOutboxSignal outboxSignal
)
{
  public async Task<Results<Ok, NotFound>> ExecuteAsync(
    AddBoardGameToShoppingBasketDto dto
  , CancellationToken cancellationToken)
  {
    ShoppingBasket? sb =
      await db.Baskets
        .SingleOrDefaultAsync(b => b.Id == dto.ShoppingBasketId, cancellationToken);
    if (sb is not null)
    {
      GameDto? game = await querySender.AskAsync(new GetGameByIdQuery(dto.BoardGameId));
      if (game is not null)
      {
        sb.AddGame(dto.BoardGameId, new Money(dto.Price, Currency.Parse(dto.Currency)));
        BoardGameSelectedForShoppingBasketIntegrationEvent e = new(
          EventId: Guid.NewGuid()
        , ShoppingBasketId: dto.ShoppingBasketId
        , BoardGameId: dto.BoardGameId
        , BoardGameName: game.GameName
        , PriceInEuro: dto.Price
        );
        await db.SaveChangesAsync(e, outboxSignal, cancellationToken);
        return TypedResults.Ok();
      }
    }
    return TypedResults.NotFound();
  }
}
