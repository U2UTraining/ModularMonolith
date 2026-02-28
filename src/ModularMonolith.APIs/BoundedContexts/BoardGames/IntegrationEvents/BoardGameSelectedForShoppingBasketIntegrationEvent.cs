namespace ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;

public sealed record class BoardGameSelectedForShoppingBasketIntegrationEvent(
  int ShoppingBasketId
, int BoardGameId
, string BoardGameName
, decimal PriceInEuro
)
: IIntegrationEvent
;
