namespace ModularMonolithBoundedContexts.Shopping.DomainEvents;

public sealed record class ShoppingBasketHasBeenCreatedDomainEvent(
  ShoppingBasket basket
)
: IDomainEvent
{ }
