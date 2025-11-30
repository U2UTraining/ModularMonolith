namespace ModularMonolith.APIs.BoundedContexts.BoardGames.QueryHandlers;

internal sealed class GetPublisherWithGamesQueryHandler
  : IQueryHandler<GetPublisherWithGamesQuery, Publisher?>
{
  private readonly PublisherRepository _repo;

  public GetPublisherWithGamesQueryHandler(PublisherRepository repo)
  {
    _repo = repo;
  }

  public async Task<Publisher?> HandleAsync(
    GetPublisherWithGamesQuery query
  , CancellationToken cancellationToken = default)
  {
    ISpecification<Publisher> spec =
      PublisherSpecification.WithId(query.PublisherId)
                           .Include(pub => pub.Games);
    Publisher? pub = await _repo.SingleAsync(spec);
    return pub;
  }
}
