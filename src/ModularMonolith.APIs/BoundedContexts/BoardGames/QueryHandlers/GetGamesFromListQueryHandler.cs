namespace ModularMonolithBoundedContexts.BoardGames.QueryHandlers;

public sealed class GetGamesFromListQueryHandler
: IQueryHandler<GetGamesFromListQuery, IQueryable<BoardGame>>
{
  private readonly IBoardGameRepository _repo;

  public GetGamesFromListQueryHandler(IBoardGameRepository repo) 
  => _repo = repo;

  public async Task<IQueryable<BoardGame>> HandleAsync(
    GetGamesFromListQuery request
  , CancellationToken cancellationToken)
  => await _repo.GetBoardGamesFromList(
    request.GameIds
  , cancellationToken);
}
