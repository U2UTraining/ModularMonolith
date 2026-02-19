namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

[Register(
  interfaceType: typeof(IQueryHandler<GetGamesQuery, IQueryable<BoardGame>>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]

internal sealed class GetGamesQueryHandler
: IQueryHandler<GetGamesQuery, IQueryable<BoardGame>>
{
  private readonly IReadonlyRepository<BoardGame> _repo;

  public GetGamesQueryHandler(IReadonlyRepository<BoardGame> repo)
  => _repo = repo;

  public async Task<IQueryable<BoardGame>> HandleAsync(
    GetGamesQuery request
  , CancellationToken cancellationToken = default)
  {
    ISpecification<BoardGame> spec = 
      BoardGameSpecification.WithPriceBetween(request.MinAmount, request.MaxAmount)
        .AsNoTracking();
    if (request.IncludePublisher)
    {
      spec = spec.Include(g => g.Publisher);
    }
    IQueryable<BoardGame> games = 
      await _repo.ListAsync(spec, cancellationToken)
        .ConfigureAwait(false);
    return games;
  }
}
