using ModularMonolith.APIs.BoundedContexts.Common.Specifications;
using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Specifications;

public static class PublisherSpecification
{
  public static ISpecification<Publisher> WithId(PK<int> id)
  {
    return new Specification<Publisher>(pub => pub.Id == id);
  }
}

// Specification pattern with extensions methods

//public static class PublisherSpecification {

//  extension(GamesDb db)
//  {
//    public IQueryable<Publisher> PublisherAggregate
//    => db.Publishers.Include(p => p.Contacts);

//    public async Task<IEnumerable<Publisher>> AllPublishers()
//    {
//      return await db.PublisherAggregate.ToListAsync();
//    }

//    public async Task<Publisher?> PublisherWithId(PK<int> id)
//    => await db.PublisherAggregate.SingleOrDefaultAsync(p => p.Id == id);
//  }
//}
