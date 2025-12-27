namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

/// <summary>
/// Query to retrieve all board games.
/// </summary>
public sealed record class GetGamesQuery(
  decimal MinAmount = decimal.Zero
, decimal MaxAmount = decimal.MaxValue
, bool IncludePublisher = false
)
: IQuery<IQueryable<BoardGame>>
{
  ///// <summary>
  ///// Factory to retrieve query instance for all games not including the publisher.
  ///// </summary>
  //public static GetGamesQuery All { get; } = new();

  ///// <summary>
  ///// Factory to retrieve query instance for all games with publisher included.
  ///// </summary>
  //public static GetGamesQuery WithPublisher { get; } = new(IncludePublisher: true);
}
