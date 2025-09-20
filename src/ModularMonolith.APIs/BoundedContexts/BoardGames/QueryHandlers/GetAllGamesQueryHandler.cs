namespace ModularMonolithBoundedContexts.BoardGames.QueryHandlers;

public sealed class GetAllGamesQueryHandler
: IQueryHandler<GetAllGamesQuery, IQueryable<BoardGame>>
{
  private readonly IReadonlyRepository<BoardGame> _repo;

  public GetAllGamesQueryHandler(IReadonlyRepository<BoardGame> repo)
  => _repo = repo;

  public async Task<IQueryable<BoardGame>> HandleAsync(GetAllGamesQuery request, CancellationToken cancellationToken)
  {
    ISpecification<BoardGame> spec = Specification<BoardGame>.All();
    if (request.IncludePublisher)
    {
      spec = spec.Include(g => g.Publisher);
    }
    IQueryable<BoardGame> games = 
      await _repo.ListAsync(spec, cancellationToken).ConfigureAwait(false);
    return games;
  }
}
