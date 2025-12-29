namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

/// <summary>
/// Query to retrieve all publishers.
/// </summary>
public sealed record class GetAllPublishersQuery(
  bool IncludeGames)
: IQuery<IQueryable<Publisher>>
;
