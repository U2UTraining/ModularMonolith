namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Repositories;

public sealed class PublisherRepository
: Repository<Publisher, BoardGamesDb>
{
  public PublisherRepository(
    BoardGamesDb dbContext
  , IDomainEventPublisher domainEventPublisher) 
  : base(dbContext, domainEventPublisher)
  { }

  //protected override IQueryable<Publisher> Includes(IQueryable<Publisher> q)
  //=> q.Include(pub => pub.Contacts);
}
