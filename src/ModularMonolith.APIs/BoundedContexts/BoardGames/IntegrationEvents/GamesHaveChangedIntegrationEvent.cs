namespace ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;

/// <summary>
/// All board games received a discount.
/// </summary>
public sealed record class GamesHaveChangedIntegrationEvent(
  Guid EventId
)
: IIntegrationEvent
;
