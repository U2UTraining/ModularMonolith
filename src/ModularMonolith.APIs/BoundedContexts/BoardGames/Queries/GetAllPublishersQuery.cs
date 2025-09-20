namespace ModularMonolith.BoundedContexts.BoardGames.Queries;

/// <summary>
/// Query to retrieve all publishers.
/// </summary>
public sealed class GetAllPublishersQuery
: IQuery<IQueryable<Publisher>>
{
  private GetAllPublishersQuery() { }

  /// <summary>
  /// Factory returning the query instance that retrieves all publishers.
  /// </summary>
  public static GetAllPublishersQuery WithGames { get; } = new();
}
