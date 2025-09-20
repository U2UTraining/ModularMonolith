using Azure.Core;

using ModularMonolithBoundedContexts.Shopping.Infra;
using ModularMonolithBoundedContexts.Shopping.Specifications;

namespace ModularMonolithBoundedContexts.Shopping.Repositories;


public sealed class ShoppingRepository
: Repository<ShoppingBasket, ShoppingDb>
, IShoppingRepository
{
  public ShoppingRepository(
    ShoppingDb db
    , IDomainEventPublisher domainEventPublisher
  ) : base(db, domainEventPublisher) { }


  public async ValueTask<ShoppingBasket?> GetShoppingBasketAsync(
    PK<int> shoppingBasketId
  , CancellationToken cancellationToken) 
  => await SingleAsync(
      ShoppingBasketSpecification.WithId(shoppingBasketId)
    , cancellationToken);

  public async ValueTask<int> DeleteOldBasketsAsync(
    CancellationToken cancellationToken)
  {
    return await DbContext
      .Baskets
      .Where(sb => EF.Property<bool>(sb, SoftDeleteable.IsDeleted) == true)
      .ExecuteDeleteAsync(cancellationToken);
  }

}
