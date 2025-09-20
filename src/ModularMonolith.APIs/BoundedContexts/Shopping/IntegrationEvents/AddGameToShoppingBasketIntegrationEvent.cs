namespace ModularMonolithBoundedContexts.Shopping.IntegrationEvents;

public sealed record class ShoppingBasketHasNewGameIntegrationEvent(
  int GameId
, string GameName
, decimal PriceInEuro
)
: IIntegrationEvent
{ }
