using ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.Entities;

public sealed class BasketItem
: EntityBase<PK<int>>
, IAggregate<ShoppingBasket>
, IHistory
, ISoftDeletable
{
  public BasketItem(PK<int> id)
  : base(id) 
  { }

  public PK<int> BoardGameId { get; set; }

  public Money Price { get; set; }
}
