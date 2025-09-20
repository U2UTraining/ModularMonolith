﻿namespace ModularMonolith.BoundedContexts.BoardGames.Queries;

/// <summary>
/// Query to retrieve a list of board games based on their IDs.
/// </summary>
/// <param name="GameIds"></param>
public sealed record class GetGamesFromListQuery(
  PK<int>[] GameIds
)
: IQuery<IQueryable<BoardGame>>
{
}
