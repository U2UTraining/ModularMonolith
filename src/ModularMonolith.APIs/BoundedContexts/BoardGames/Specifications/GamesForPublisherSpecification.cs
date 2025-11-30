using ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;
using ModularMonolith.APIs.BoundedContexts.Common.Specifications;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Specifications;

public sealed class GamesForPublisherSpecification 
: Specification<BoardGame>
{
  public GamesForPublisherSpecification(int publisherID)
    : base(game => EF.Property<int>(game, "PublisherId") == publisherID)
  { }
}
