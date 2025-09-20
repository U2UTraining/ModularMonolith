namespace U2U.ModularMonolith.BoundedContexts.Shopping.IntegrationEvents;

public sealed record class ShoppingBasketHasNewGameIntegrationEvent(
  int GameId
, string GameName
, decimal PriceInEuro
)
: IIntegrationEvent
{ }
