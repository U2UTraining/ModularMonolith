namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

[Register(
  interfaceType: typeof(IQueryHandler<GetGamesFromListQuery, List<BoardGame>>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]
internal sealed class GetGamesFromListQueryHandler(BoardGamesDb db)
: IQueryHandler<GetGamesFromListQuery, List<BoardGame>>
{
  public async Task<List<BoardGame>> HandleAsync(
    GetGamesFromListQuery request
  , CancellationToken cancellationToken = default)
  => await db.BoardGames
    .Include(game => game.Image)
    .Include(game => game.Publisher)
    .Where(game => request.GameIds.Contains(game.Id))
    .ToListAsync(cancellationToken);
}
