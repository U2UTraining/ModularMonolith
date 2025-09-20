namespace ModularMonolithBoundedContexts.Shopping.Entities;

public sealed class BasketItem
: EntityBase<PK<int>>
, IAggregate<ShoppingBasket>
{
  public BasketItem(PK<int> id)
  : base(id) 
  { }

  public PK<int> BoardGameId { get; set; }

  public Money Price { get; set; }
}
