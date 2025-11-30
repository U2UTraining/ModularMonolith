using ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;
using ModularMonolith.APIs.BoundedContexts.Common.Specifications;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Specifications;

public sealed class AllGamesSpecification : Specification<BoardGame>
{
  public AllGamesSpecification()
  : base((_) => true)
  { }
}
