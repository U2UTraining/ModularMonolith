namespace ModularMonolith.APIs.BoundedContexts.BoardGames.CommandHandlers;

internal sealed class AddBoardGameToPublisherCommandHandler
: ICommandHandler<AddBoardGameToPublisherCommand, Publisher>
{
  private readonly GamesDb _db;
  private readonly IRepository<Publisher> _repo;
  private readonly IIntegrationEventPublisher _publisher;

  public AddBoardGameToPublisherCommandHandler(
    GamesDb db
  , IRepository<Publisher> repo
  , IIntegrationEventPublisher publisher)
  {
    _db = db;
    _repo = repo;
    _publisher = publisher;
  }

  public async Task<Publisher> HandleAsync(
    AddBoardGameToPublisherCommand request
  , CancellationToken cancellationToken)
  {
    ISpecification<Publisher> spec =
      PublisherSpecification.WithId(request.PublisherId);
    Publisher? publisher = await _repo.SingleAsync(spec, cancellationToken).ConfigureAwait(false);
    //Publisher? publisher = await _db.PublisherWithId(request.PublisherId);
    if (publisher is not null)
    {
      BoardGame game = publisher.CreateGame(request.Name, request.PriceInEuro);
      await _repo.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      await _publisher.PublishIntegrationEventAsync(
        new NewBoardGameCreatedIntegrationEvent(
           GameId: game.Id.Key,
           BoardGameName: game.Name.Value,
           PriceInEuro: game.Price.Amount
      ), cancellationToken).ConfigureAwait(false);
      return publisher;
    }
    throw new ArgumentException(
      message: $"Publisher with id {request.PublisherId} could not be found"
    , paramName: nameof(request));
  }
}
