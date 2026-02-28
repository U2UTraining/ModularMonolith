namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShoppingServices")]
public sealed class AddBoardGameToShoppingBasket(
  ShoppingDb db
, IQuerySender querySender
, IIntegrationEventPublisher eventPublisher)
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
      BoardGame? game = await querySender.AskAsync(new GetGameByIdQuery(dto.BoardGameId));
      if (game is not null)
      {
        sb.AddGame(dto.BoardGameId, new Money(dto.PriceInEuro));
        await db.SaveChangesAsync(cancellationToken);
        BoardGameSelectedForShoppingBasketIntegrationEvent e = new(
          ShoppingBasketId: dto.ShoppingBasketId
        , BoardGameId: dto.BoardGameId
        , BoardGameName: game.Name.Value
        , PriceInEuro: dto.PriceInEuro
        );
        await eventPublisher.PublishIntegrationEventAsync(e);
        return TypedResults.Ok();
      }
    }
    return TypedResults.NotFound();
  }
}
