using ModularMonolith.APIs.BoundedContexts.Common.DomainEvents;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.DomainEvents;

public sealed record class ShoppingBasketHasBeenCreatedDomainEvent(
  ShoppingBasket basket
)
: IDomainEvent
{ }
