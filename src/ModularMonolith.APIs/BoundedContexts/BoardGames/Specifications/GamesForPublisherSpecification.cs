﻿namespace U2U.ModularMonolith.BoundedContexts.BoardGames.Specifications;

public sealed class GamesForPublisherSpecification 
: Specification<BoardGame>
{
  public GamesForPublisherSpecification(int publisherID)
    : base(game => EF.Property<int>(game, "PublisherId") == publisherID)
  { }
}
