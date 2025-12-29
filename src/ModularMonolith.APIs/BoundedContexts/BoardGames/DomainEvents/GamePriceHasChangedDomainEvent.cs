namespace ModularMonolith.APIs.BoundedContexts.BoardGames.DomainEvents;

public sealed record class GamePriceHasChangedDomainEvent(
  BoardGame Game)
: IDomainEvent
;
