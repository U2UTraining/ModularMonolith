using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.IntegrationEvents;

public sealed record class ShoppingBasketHasNewGameIntegrationEvent(
  int GameId
, string GameName
, decimal PriceInEuro
)
: IIntegrationEvent
{ }
