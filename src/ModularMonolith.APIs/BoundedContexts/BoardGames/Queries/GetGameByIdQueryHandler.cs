namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

[Register(
  interfaceType: typeof(IQueryHandler<GetGameByIdQuery, BoardGame?>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]
internal sealed class GetGameByIdQueryHandler(BoardGamesDb db)
  : IQueryHandler<GetGameByIdQuery, BoardGame?>
{
  public async Task<BoardGame?> HandleAsync(
    GetGameByIdQuery query
  , CancellationToken cancellationToken = default)
  {
    return await db.BoardGames.FindAsync([ new PK<int>(query.GameId) ], cancellationToken);
  }
}
