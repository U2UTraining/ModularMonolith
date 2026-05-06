namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

[Register(
  serviceType: typeof(IQueryHandler<GetGameByIdQuery, GameDto?>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]
internal sealed class GetGameByIdQueryHandler(BoardGamesDb db)
  : IQueryHandler<GetGameByIdQuery, GameDto?>
{
  //public async Task<BoardGame?> HandleAsync(
  //  GetGameByIdQuery query
  //, CancellationToken cancellationToken = default)
  //{
  //  return await db.BoardGames.FindAsync([ new PK<int>(query.GameId) ], cancellationToken);
  //}

  public async Task<GameDto?> HandleAsync(
  GetGameByIdQuery query
, CancellationToken cancellationToken = default)
  {
    return await db.BoardGames
      .Include(g => g.Image)
      .Include(g => g.Publisher)
      .AsNoTracking()
      .Where(g => g.Id == query.GameId)
      .Select(g => new GameDto(
        g.Id
      , g.Name
      , g.Price.Amount
      , g.Price.Currency
      , g.ImageURL
      , g.PublisherName))
      .SingleOrDefaultAsync(cancellationToken);
  }
}
