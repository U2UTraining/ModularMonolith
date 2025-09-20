namespace ModularMonolithBoundedContexts.Shopping.DomainEvents;

public sealed record class ShoppingBasketHasNewGameDomainEvent(
  ShoppingBasket basket
, PK<int> GameId
)
: IDomainEvent
{ }
