﻿namespace ModularMonolith.BoundedContexts.Shopping.DomainEvents;

public sealed record class ShoppingBasketHasNewGameDomainEvent(
  ShoppingBasket basket
, PK<int> GameId
)
: IDomainEvent
{ }
