namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddShoppingServices")]
public sealed class GetShoppingBasketWithId(IQuerySender querySender)
{
  public async Task<Results<Ok<ShoppingBasketDto>, NotFound>> ExecuteAsync(
    int id
  , CancellationToken cancellationToken)
  {
    ShoppingBasketDto? dto = await querySender.AskAsync(
      new ShoppingBasketWithIdQuery(id, IncludeGames: true)
    , cancellationToken);
    if (dto is null)
    {

      return TypedResults.NotFound();
    }
    else
    {
      return TypedResults.Ok(dto);
    }
  }
}
