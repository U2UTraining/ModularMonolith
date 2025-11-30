using ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.Entities;

public sealed class ShoppingBasket
: EntityBase<PK<int>>
, IAggregateRoot
, IHistory
, ISoftDeletable
{
  public ShoppingBasket(PK<int> id) 
  : base(id) 
  {
    this.RegisterDomainEvent(new ShoppingBasketHasBeenCreatedDomainEvent(this));
  }

  public Customer? Customer { get; private set; } = default!;

  //public int? CustomerId { get; set; }

  public void AssignCustomer(
    FirstName firstName
  , LastName lastName
  , StreetName street
  , CityName city)
  {
    Customer = new Customer(default, firstName, lastName);
    Address address = new Address(street, city);
    Customer.MoveToNewAddress(address);
  }

  public void CheckOut()
  {
    Customer = new Customer(default, new FirstName( "Jefke"), new LastName("Versmossen"));
    Customer.MoveToNewAddress(new Address(new StreetName("ResearchPark 110"), new CityName("Zellik")));
    RegisterDomainEvent(
      new ShoppingBasketHasCheckedOutDomainEvent(
        ShoppingBasketId: Id));
  }

  public void AddGame(PK<int> boardGameId, Money price)
  {
    GetBasket()
      .Add(new BasketItem(default) 
      { 
        BoardGameId = boardGameId
      , Price = price 
      });
    RegisterDomainEvent(
      new ShoppingBasketHasNewGameDomainEvent(
        this,
        boardGameId));
  }

  private ICollection<BasketItem> GetBasket()
    => gamesInBasket ??= new List<BasketItem>();

  public void Remove(int boardGameId)
  {
    BasketItem? gameInBasket = 
      gamesInBasket.SingleOrDefault(g => g.BoardGameId.Key == boardGameId);
    if (gameInBasket is not null)
    {
      gamesInBasket.Remove(gameInBasket);
    }
  }

  //public IEnumerable<Game> Games
    //=> gamesInBasket?.Select(gib => gib.Game) ?? Enumerable.Empty<Game>();

  public IEnumerable<BasketItem> Items
    => gamesInBasket ?? [];

  private ICollection<BasketItem> gamesInBasket = default!;
}
