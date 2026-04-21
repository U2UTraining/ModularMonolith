namespace ModularMonolith.APIs.BoundedContexts.Shopping.Queries;

public class ShoppingBasketsWithStateQuery
  : IQuery<List<ShoppingBasket>>
{
  public ShoppingBasketState State
  {
    get; init;
  }

  public static ShoppingBasketsWithStateQuery Open
  {
    get;
  } = new()
  {
    State = ShoppingBasketState.Open
  };
}
