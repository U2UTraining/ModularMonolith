namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShoppingServices")]
public sealed class CreateShoppingBasket(ShoppingDb db)
{
  public async Task<Results<Ok<int>, NotFound>> ExecuteAsync(
    CancellationToken cancellationToken)
  {
    ShoppingBasket newShoppingBasket = new(0);
    db.Baskets.Add(newShoppingBasket);
    await db.SaveChangesAsync(cancellationToken);
    return TypedResults.Ok(newShoppingBasket.Id.Key);
  }
}
