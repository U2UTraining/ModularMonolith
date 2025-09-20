namespace ModularMonolith.BoundedContexts.Shopping.Specifications;

public static class ShoppingBasketSpecification
{
  public static ISpecification<ShoppingBasket> WithId(PK<int> id)
=> new Specification<ShoppingBasket>(g => g.Id == id);

}
