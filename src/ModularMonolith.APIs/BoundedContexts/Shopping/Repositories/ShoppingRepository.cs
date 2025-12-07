
using System.Xml.Linq;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.Repositories;

//internal sealed class ShoppingRepository
//: Repository<ShoppingBasket, ShoppingDb>
//, IShoppingRepository
//{
//  public ShoppingRepository(
//    ShoppingDb db
//    , IDomainEventPublisher domainEventPublisher
//  ) : base(db, domainEventPublisher) { }


//  public async ValueTask<ShoppingBasket?> GetShoppingBasketAsync(
//    PK<int> shoppingBasketId
//  , CancellationToken cancellationToken) 
//  => await SingleAsync(
//      ShoppingBasketSpecification.WithId(shoppingBasketId)
//    , cancellationToken);

//  public async ValueTask<int> DeleteOldBasketsAsync(
//    CancellationToken cancellationToken)
//  {
//    return await DbContext
//      .Baskets
//      .Where(sb => EF.Property<bool>(sb, SoftDeleteable.IsDeleted) == true)
//      .ExecuteDeleteAsync(cancellationToken);
//  }

//  protected override IQueryable<ShoppingBasket> Includes(IQueryable<ShoppingBasket> q)
//  {
//    q = q.Include(sb => sb.Items).ThenInclude(it => it.G);
//    q = q.Include(sb => sb.Customer);
//    return q;
//  }
//}

internal static class ShoppingRepostory
{
  extension(ShoppingDb db)
  {
    private IQueryable<ShoppingBasket> ShoppingBasketAggregate(
      bool includeGames = false
    , bool includeCustomer = false
    )
    {
      IQueryable<ShoppingBasket> query = db.Baskets;
      if (includeGames)
      {
        query = query.Include(sb => sb.Items);
      }
      if (includeCustomer)
      {
        query = query.Include(sb => sb.Customer);
      }
      return query;
    }

    public async ValueTask<ShoppingBasket?> GetShoppingBasketAsync(
      PK<int> shoppingBasketId
    , bool includeGames = false
    , bool includeCustomer = false
    , CancellationToken cancellationToken = default)
    {
      return await db.ShoppingBasketAggregate(includeGames: includeGames, includeCustomer: includeCustomer)
        .AsNoTracking()
        .Where(sb => sb.Id == shoppingBasketId)
        .SingleOrDefaultAsync(cancellationToken);
    }

    public async ValueTask<int> DeleteOldBasketsAsync(
      CancellationToken cancellationToken)
    {
      return await db
        .Baskets
        .Where(sb => EF.Property<bool>(sb, SoftDeleteable.IsDeleted) == true)
        .ExecuteDeleteAsync(cancellationToken);
    }
  }
}
