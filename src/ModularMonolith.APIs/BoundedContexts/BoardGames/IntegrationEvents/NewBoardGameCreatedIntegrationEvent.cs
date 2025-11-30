using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;

/// <summary>
/// A new board game has been created.
/// </summary>
/// <param name="GameId">PK of board game</param>
/// <param name="BoardGameName">Name</param>
/// <param name="PriceInEuro">Price</param>
public sealed record class NewBoardGameCreatedIntegrationEvent(
  int GameId
, string BoardGameName
, decimal PriceInEuro
)
: IIntegrationEvent
{ }
