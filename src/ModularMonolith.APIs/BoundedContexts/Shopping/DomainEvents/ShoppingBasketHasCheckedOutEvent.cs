namespace ModularMonolith.APIs.BoundedContexts.Shopping.DomainEvents;

public sealed record class ShoppingBasketHasCheckedOutDomainEvent(
  PK<int> ShoppingBasketId)
: IDomainEvent
{ }
