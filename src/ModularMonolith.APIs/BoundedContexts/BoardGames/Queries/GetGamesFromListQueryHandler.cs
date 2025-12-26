namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

internal sealed class GetGamesFromListQueryHandler
: IQueryHandler<GetGamesFromListQuery, IQueryable<BoardGame>>
{
  private readonly IBoardGameRepository _repo;

  public GetGamesFromListQueryHandler(IBoardGameRepository repo) 
  => _repo = repo;

  public async Task<IQueryable<BoardGame>> HandleAsync(
    GetGamesFromListQuery request
  , CancellationToken cancellationToken = default)
  => await _repo.GetBoardGamesFromList(
    request.GameIds
  , cancellationToken);
}
