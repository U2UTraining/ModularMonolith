using ModularMonolith.APIs.BoundedContexts.Common.DomainEvents;
using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.DomainEvents;

public sealed record class ShoppingBasketHasNewGameDomainEvent(
  ShoppingBasket basket
, PK<int> GameId
)
: IDomainEvent
{ }
