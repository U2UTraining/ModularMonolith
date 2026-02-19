namespace ModularMonolith.APIs.BoundedContexts.Shopping.Queries;

[Register(
  interfaceType: typeof(IQueryHandler<ShoppingBasketWithIdQuery, ShoppingBasketDto?>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShoppingServices")]
internal sealed class ShoppingBasketWithIdQueryHandler
  : IQueryHandler<ShoppingBasketWithIdQuery, ShoppingBasketDto?>
{
  private readonly ShoppingDb _db;
  private readonly IQuerySender _querySender;

  public ShoppingBasketWithIdQueryHandler(
    ShoppingDb db
  , IQuerySender querySender)
  {
    _db = db;
    _querySender = querySender;
  }

  public async Task<ShoppingBasketDto?> HandleAsync(
    ShoppingBasketWithIdQuery query
  , CancellationToken cancellationToken = default)
  {
    ShoppingBasket? basket = await _db.GetShoppingBasketAsync(
      query.ShoppingBasketId
    , includeGames: query.IncludeGames
    , includeCustomer: query.IncludeCustomer
    , cancellationToken
      );

    if (basket is null)
    {
      return null;
    }

    if (query.IncludeGames)
    {
      int[] games = basket.Items.Select(item => item.BoardGameId.Key).ToArray();
      IQueryable<BoardGame> gameItems = await _querySender.AskAsync(new GetGamesFromListQuery(games), cancellationToken);
      ShoppingBasketDto dto = ShoppingBasketDto.ToDTO(basket, gameItems);
      return dto;
    } else
    {
      ShoppingBasketDto dto = ShoppingBasketDto.ToDTO(basket, null);
      return dto;
    }
  }
}
