namespace ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;

public record class BoardGameSelectedForShoppingBasketIntegrationEvent(
  int ShoppingBasketId
, int BoardGameId
, decimal PriceInEuro
)
: IIntegrationEvent;
