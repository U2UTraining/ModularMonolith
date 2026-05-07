namespace ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;

public sealed record class BoardGamePriceUpdateEvent(
  Guid EventId
, PK<int> BoardGameId
, Money PriceInEuro
)
: IIntegrationEvent
{ }
