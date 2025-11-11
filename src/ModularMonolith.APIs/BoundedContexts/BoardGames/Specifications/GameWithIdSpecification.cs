using ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Specifications;

public sealed class GameWithIdSpecification : Specification<BoardGame>
{
  public GameWithIdSpecification(PK<int> gameId)
  : base(game => game.Id == gameId)
  { }
}
