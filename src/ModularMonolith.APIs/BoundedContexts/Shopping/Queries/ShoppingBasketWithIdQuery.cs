namespace ModularMonolith.APIs.BoundedContexts.Shopping.Queries;

public sealed record class ShoppingBasketWithIdQuery(
  int ShoppingBasketId
, bool IncludeGames = false
, bool IncludeCustomer = false
)
: IQuery<ShoppingBasketDto?>;
