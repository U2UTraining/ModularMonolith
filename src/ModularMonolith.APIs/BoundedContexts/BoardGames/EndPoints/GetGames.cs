namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

/// <summary>
/// Handler for GetGamesQuery
/// </summary>
/// <param name="db"></param>
[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGames")]

public sealed class GetGames(GamesDb db)
{
  public async Task<Results<Ok<List<GameDto>>, BadRequest>> ExecuteAsync(GetGamesQuery query, CancellationToken cancellationToken)
  {
    IQueryable<BoardGame> gamesQuery = 
      db.Games
        .AsNoTracking()
        .Include(g => g.Image);
    if( query.MinAmount > 0)
    {
      gamesQuery = gamesQuery.Where(g => g.Price.Amount >= query.MinAmount);
    }
    if ((query.MaxAmount < decimal.MaxValue))
    {
      gamesQuery = gamesQuery.Where(g => g.Price.Amount <= query.MaxAmount);
    }
    if( query.IncludePublisher)
    {
      gamesQuery = gamesQuery.Include(g => g.Publisher);
    }
    List<GameDto> games = await gamesQuery
      .Select(g => new GameDto
      (
          Id: g.Id
        , GameName: g.Name.Value
        , Price: g.Price.Amount
        , ImageURL: g.ImageURL
        , PublisherName: query.IncludePublisher ? g.PublisherName : string.Empty
      ))
      .ToListAsync(cancellationToken);
    return TypedResults.Ok(games);
  }
}
