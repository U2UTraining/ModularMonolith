namespace EFCore.Core.Shopping.DomainEvents;

public sealed record class ShoppingBasketHasCheckedOutEvent(
  PK<int> ShoppingBasketId)
: IDomainEvent
, IIntegrationEvent // TODO Make this separate IE
{ }
