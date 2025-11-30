using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;

public sealed record class BoardGameSelectedForShoppingBasketIntegrationEvent(
  int ShoppingBasketId
, int BoardGameId
, decimal PriceInEuro
)
: IIntegrationEvent;
