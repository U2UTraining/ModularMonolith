using ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;
using ModularMonolith.APIs.BoundedContexts.Common.Specifications;
using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Specifications;

public sealed class GameWithIdSpecification : Specification<BoardGame>
{
  public GameWithIdSpecification(PK<int> gameId)
  : base(game => game.Id == gameId)
  { }
}
