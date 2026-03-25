namespace ModularMonolith.APIs.BoundedContexts.Shopping.IntegrationEvents;

public sealed record class ShoppingBasketHasNewGameIntegrationEvent(
  Guid EventId
, int GameId
, string GameName
, decimal PriceInEuro
)
: IIntegrationEvent
{
}
