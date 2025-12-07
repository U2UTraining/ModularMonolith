namespace ModularMonolith.APIs.BoundedContexts.Shopping.Queries;

public sealed record class ShoppingBasketWithIdQuery(
  int ShoppingBasketId
, bool includeGames = false
, bool includeCustomer = false
)
  : IQuery<ShoppingBasketDTO?>;
