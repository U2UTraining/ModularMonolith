using ModularMonolith.APIs.EFCore.Auditability;
using ModularMonolith.APIs.EFCore.SoftDelete;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.Entities;

public sealed class BasketItem
: EntityBase<PK<int>>
, IAggregate<ShoppingBasket>
, IAuditability
, ISoftDeletable
{
  public BasketItem(PK<int> id)
  : base(id) 
  { }

  public PK<int> BoardGameId { get; set; }

  public Money Price { get; set; }
}
