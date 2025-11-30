using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

/// <summary>
/// Query to retrieve all board games.
/// </summary>
public sealed class GetAllGamesQuery
: IQuery<IQueryable<BoardGame>>
{
  private GetAllGamesQuery(bool withPublisher) 
  => IncludePublisher = withPublisher;

  public bool IncludePublisher { get; }

  /// <summary>
  /// Factory to retrieve query instance for all games without publisher.
  /// </summary>
  public static GetAllGamesQuery Instance { get; } = new(false);

  /// <summary>
  /// Factory to retrieve query instance for all games with publisher included.
  /// </summary>
  public static GetAllGamesQuery WithPublisher { get; } = new(true);
}
