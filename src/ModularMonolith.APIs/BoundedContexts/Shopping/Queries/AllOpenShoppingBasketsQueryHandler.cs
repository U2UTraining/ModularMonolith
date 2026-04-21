namespace ModularMonolith.APIs.BoundedContexts.Shopping.Queries;

[Register(
  serviceType: typeof(IQueryHandler<ShoppingBasketsWithStateQuery, List<ShoppingBasket>>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShoppingServices")]
public class ShoppingBasketsWithStateQueryHandler(ShoppingDb db)
  : IQueryHandler<ShoppingBasketsWithStateQuery, List<ShoppingBasket>>
{
  public async Task<List<ShoppingBasket>> HandleAsync(
    ShoppingBasketsWithStateQuery query
  , CancellationToken cancellationToken)
  {
    return await db.Baskets
        .Where(sb => sb.State == query.State)
        .ToListAsync(cancellationToken);
  }
}
