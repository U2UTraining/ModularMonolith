namespace U2U.ModularMonolith.BoundedContexts.Shopping.Entities;

public sealed class Customer 
: EntityBase<PK<int>>
, IAggregate<ShoppingBasket>
, IHistory
, ISoftDeletable
{
  [SetsRequiredMembers]
  internal Customer(
    PK<int> id
  , FirstName firstName
  , LastName lastName)
  : base(id)
  {
    FirstName = firstName;
    LastName = lastName;
  }

  public required FirstName FirstName { get; init; }

  public required LastName LastName { get; init; }

  public Address Address { get; private set; }

  public void MoveToNewAddress(in Address address)
    => Address = address;
}

