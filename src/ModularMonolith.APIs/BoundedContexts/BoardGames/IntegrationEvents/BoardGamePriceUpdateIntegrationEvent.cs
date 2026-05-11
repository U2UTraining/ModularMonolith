namespace ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;

public sealed record class BoardGamePriceUpdateIntegrationEvent(
  Guid EventId
, PK<int> BoardGameId
, BoardGameName Name
, Money PriceInEuro
)
: IIntegrationEvent
{ }
