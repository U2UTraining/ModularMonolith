namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

[Register(
  serviceType: typeof(IQueryHandler<GetGamesFromListQuery, List<GameDto>>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]
internal sealed class GetGamesFromListQueryHandler(BoardGamesDb db)
: IQueryHandler<GetGamesFromListQuery, List<GameDto>>
{
  public async Task<List<GameDto>> HandleAsync(
    GetGamesFromListQuery request
  , CancellationToken cancellationToken = default)
  => await db.BoardGames
    .Include(game => game.Image)
    .Include(game => game.Publisher)
    .Where(game => request.GameIds.Contains(game.Id))
          .Select(g => new GameDto(
        g.Id
      , g.Name
      , g.Price.Amount
      , g.Price.Currency
      , g.ImageURL
      , g.PublisherName))
    .ToListAsync(cancellationToken);
}
