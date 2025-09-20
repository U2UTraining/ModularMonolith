namespace ModularMonolith.BoundedContexts.Shopping.DomainEvents;

public sealed record class ShoppingBasketHasBeenCreatedDomainEvent(
  ShoppingBasket basket
)
: IDomainEvent
{ }
