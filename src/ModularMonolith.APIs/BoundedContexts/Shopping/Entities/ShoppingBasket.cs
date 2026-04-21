namespace ModularMonolith.APIs.BoundedContexts.Shopping.Entities;

public sealed class ShoppingBasket
: EntityBase<PK<int>>
, IAggregateRoot
, IAuditability
, ISoftDeletable
{
  public ShoppingBasket(PK<int> id) 
  : base(id) 
  {
    State = ShoppingBasketState.Open;
    RegisterDomainEvent(new ShoppingBasketHasBeenCreatedDomainEvent(this));
  }

  public ShoppingBasketState State
  {
    get; private set;
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
    State = ShoppingBasketState.CheckedOut;
    Customer = new Customer(default, new FirstName( "Jefke"), new LastName("Versmossen"));
    Customer.MoveToNewAddress(new Address(new StreetName("ResearchPark 110"), new CityName("Zellik")));
    RegisterDomainEvent(
      new ShoppingBasketHasCheckedOutDomainEvent(
        ShoppingBasketId: Id));
  }

  public void AddGame(PK<int> boardGameId, Money price)
  {
    if (State is not ShoppingBasketState.Open)
    {
      throw new Exception("Games can only be added to open shoppingbasket");
    }
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

  public void SyncGamePrices(CurrencyName currency, decimal factor)
  {
    foreach (BasketItem item in GetBasket())
    {
      if (item.Price.Currency == currency)
      {
        item.Price = item.Price * factor;
      }
    }
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
