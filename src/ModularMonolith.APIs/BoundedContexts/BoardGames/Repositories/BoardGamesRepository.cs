namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Repositories;

internal sealed class BoardGamesRepository
: Repository<BoardGame, GamesDb>
, IBoardGameRepository
{
  private readonly IIntegrationEventPublisher _integrationEventPubslisher;

  public BoardGamesRepository(
    GamesDb db
  , IDomainEventPublisher domainEventPublisher
  , IIntegrationEventPublisher integrationEventPubslisher
  ) : base(db, domainEventPublisher)
  {
    _integrationEventPubslisher = integrationEventPubslisher;
  }

  public async ValueTask ApplyMegaDiscountAsync(
    decimal discount
  , CancellationToken cancellationToken = default)
  {
    //DbSet<BoardGame> games = DbContext.Games;
    //foreach (BoardGame game in games)
    //{
    //  game.SetPrice(game.Price.WithAmount(game.Price.Amount * discount));
    //}
    //await SaveChangesAsync(cancellationToken);

    // Or use bulk update

    await DbContext.Games.ExecuteUpdateAsync(s =>
      s.SetProperty(bg => bg.Price.Amount,
                    bg => bg.Price.Amount * discount)
    , cancellationToken
    )
    .ConfigureAwait(false);
    // Now we need to update the entities that are in memory
    // We can do this using an Integration Event
    await _integrationEventPubslisher.PublishIntegrationEventAsync(
      new GamesHaveChangedIntegrationEvent(), cancellationToken
    ).ConfigureAwait(false);
  }

  public ValueTask<IQueryable<BoardGame>> GetBoardGamesFromList(
    PK<int>[] gameIds
  , CancellationToken cancellationToken)
  => ValueTask.FromResult(Includes(DbContext.Games).Where(game => gameIds.Contains(game.Id)));

  protected override IQueryable<BoardGame> Includes(IQueryable<BoardGame> q)
  {
    q = q.Include(g => g.Image);
    q = q.Include(g => g.Publisher);
    return q;
  }
}
