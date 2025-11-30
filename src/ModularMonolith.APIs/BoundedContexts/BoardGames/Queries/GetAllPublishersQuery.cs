using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

/// <summary>
/// Query to retrieve all publishers.
/// </summary>
public sealed class GetAllPublishersQuery
: IQuery<IQueryable<Publisher>>
{
  private GetAllPublishersQuery()
  {
  }

  public required bool IncludeGames { get; init; } = false;

  public static GetAllPublishersQuery Default
  {
    get;
  } = new()
  {
    IncludeGames = false
  };

  public static GetAllPublishersQuery WithGames
  {
    get;
  } = new()
  {
    IncludeGames = true
  };
}
