namespace ModularMonolith.BoundedContexts.Shopping.Repositories;

public interface IShoppingRepository
: IRepository<ShoppingBasket>
{
  ValueTask<ShoppingBasket?> GetShoppingBasketAsync(
    PK<int> i
  , CancellationToken cancellationToken);

  ValueTask<int> DeleteOldBasketsAsync(CancellationToken cancellationToken);
}
