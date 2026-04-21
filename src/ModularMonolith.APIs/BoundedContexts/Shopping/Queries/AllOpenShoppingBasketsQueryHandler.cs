namespace ModularMonolith.APIs.BoundedContexts.Shopping.Queries;

[Register(
  serviceType: typeof(IQueryHandler<ShoppingBasketsWithStateQuery, List<ShoppingBasket>>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShoppingServices")]
internal sealed class ShoppingBasketsWithStateQueryHandler(ShoppingDb db)
  : IQueryHandler<ShoppingBasketsWithStateQuery, List<ShoppingBasket>>
{
  public async Task<List<ShoppingBasket>> HandleAsync(
    ShoppingBasketsWithStateQuery query
  , CancellationToken cancellationToken)
  {
    return await db.Baskets
      .Include(sb => sb.Items)
      .Include(sb => sb.Customer)
      .Where(sb => sb.State == query.State)
      .ToListAsync(cancellationToken);
  }
}
