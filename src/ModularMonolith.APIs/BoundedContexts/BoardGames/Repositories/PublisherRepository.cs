using ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;
using ModularMonolith.APIs.BoundedContexts.Common.DomainEvents;
using ModularMonolith.APIs.BoundedContexts.Common.Repositories;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Repositories;

public sealed class PublisherRepository
: Repository<Publisher, GamesDb>
{
  public PublisherRepository(
    GamesDb dbContext
  , IDomainEventPublisher domainEventPublisher) 
  : base(dbContext, domainEventPublisher)
  { }

  protected override IQueryable<Publisher> Includes(IQueryable<Publisher> q)
  => q.Include(pub => pub.Contacts);
}
