namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

internal sealed class GetAllPublishersQueryHandler
: IQueryHandler<GetAllPublishersQuery, IQueryable<Publisher>>
{
  private readonly IReadonlyRepository<Publisher> _repo;

  public GetAllPublishersQueryHandler(IReadonlyRepository<Publisher> repo)
  => _repo = repo;

  public async Task<IQueryable<Publisher>> HandleAsync(
    GetAllPublishersQuery request
  , CancellationToken cancellationToken)
  {
    // Query Splitting?
    ISpecification<Publisher> spec = Specification<Publisher>
      .All()
      .Include(pub => pub.Games, withQuerySplitting: false);
    IQueryable<Publisher> result = 
      await _repo.ListAsync(spec, cancellationToken)
                 .ConfigureAwait(false);
    return result;
  }

//  public async Task<IQueryable<Publisher>?> Handle(
//  GetAllPublishersQuery request
//, CancellationToken cancellationToken)
//  {
//    // Query Splitting?
//    ISpecification<Publisher> spec = Specification<Publisher>
//      .All()
//      .Include(pub => pub.Games, withQuerySplitting: true);
//    IQueryable<Publisher>? result =
//      await _repo.ListAsync(spec, cancellationToken);

//    // IQueryables with query splitting do not support pagination
//    // So we turn this into a List and then again into a IQueryable

//    return (await result.ToListAsync(cancellationToken)).AsQueryable();
//  }
}
