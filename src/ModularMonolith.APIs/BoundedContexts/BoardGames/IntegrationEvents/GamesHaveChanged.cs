namespace ModularMonolith.BoundedContexts.BoardGames.IntegrationEvents;

/// <summary>
/// All board games received a discount.
/// </summary>
public sealed record class GamesHaveChanged()
: IIntegrationEvent
{ }
