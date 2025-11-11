using ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Specifications;

public sealed class AllGamesSpecification : Specification<BoardGame>
{
  public AllGamesSpecification()
  : base((_) => true)
  { }
}
