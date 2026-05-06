//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

// ==============================================================================
// Using a DbContext
// ==============================================================================

[Register(
  serviceType: typeof(IQueryHandler<GetGamesQuery, IEnumerable<BoardGame>>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]
internal sealed class GetGamesQueryHandler(BoardGamesDb db)
: IQueryHandler<GetGamesQuery, IEnumerable<BoardGame>>
{
  public async Task<IEnumerable<BoardGame>> HandleAsync(
    GetGamesQuery query
  , CancellationToken cancellationToken = default)
  {
    IQueryable<BoardGame> gamesQuery =
      db.BoardGames
        .AsNoTracking()
        .Include(g => g.Image);
    if (query.MinAmount > 0)
    {
      gamesQuery = gamesQuery.Where(g => g.Price.Amount >= query.MinAmount);
    }
    if ((query.MaxAmount < decimal.MaxValue))
    {
      gamesQuery = gamesQuery.Where(g => g.Price.Amount <= query.MaxAmount);
    }
    if (query.IncludePublisher)
    {
      gamesQuery = gamesQuery.Include(g => g.Publisher);
    }
    return await gamesQuery.ToListAsync(cancellationToken);
    ;
  }
}

// ==============================================================================
// Using a Specification & Repository
// ==============================================================================

//[Register(
//  serviceType: typeof(IQueryHandler<GetGamesQuery, IQueryable<BoardGame>>)
//, lifetime: ServiceLifetime.Scoped
//, methodNameHint: "AddBoardGameServices")]
//internal sealed class GetGamesQueryHandler
//: IQueryHandler<GetGamesQuery, IQueryable<BoardGame>>
//{
//  private readonly IReadonlyRepository<BoardGame> _repo;

//  public GetGamesQueryHandler(IReadonlyRepository<BoardGame> repo)
//  => _repo = repo;

//  public async Task<IQueryable<BoardGame>> HandleAsync(
//    GetGamesQuery request
//  , CancellationToken cancellationToken = default)
//  {
//    ISpecification<BoardGame> spec = 
//      BoardGameSpecification.WithPriceBetween(request.MinAmount, request.MaxAmount)
//        .AsNoTracking();
//    if (request.IncludePublisher)
//    {
//      spec = spec.Include(g => g.Publisher);
//    }
//    IQueryable<BoardGame> games = 
//      await _repo.ListAsync(spec, cancellationToken)
//        .ConfigureAwait(false);
//    return games;
//  }
//}
