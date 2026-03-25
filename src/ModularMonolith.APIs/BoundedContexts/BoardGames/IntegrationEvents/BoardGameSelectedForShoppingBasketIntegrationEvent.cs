namespace ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;

public sealed record class BoardGameSelectedForShoppingBasketIntegrationEvent(
  Guid EventId
, int ShoppingBasketId
, int BoardGameId
, string BoardGameName
, decimal PriceInEuro
)
: IIntegrationEvent
;
